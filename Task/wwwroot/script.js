document.addEventListener('DOMContentLoaded', () => {
    const API_BASE_URL = 'https://localhost:7234/api/Player';

    const playerStateEl = document.getElementById('playerState');
    const currentTrackNameEl = document.getElementById('currentTrackName');
    const playPauseBtn = document.getElementById('playPauseBtn');
    const stopBtn = document.getElementById('stopBtn');
    const nextBtn = document.getElementById('nextBtn');
    const fileInput = document.getElementById('fileInput');
    const addSelectedFilesBtn = document.getElementById('addSelectedFilesBtn');
    const setFirstTrackBtn = document.getElementById('setFirstTrackBtn');
    const playlistEl = document.getElementById('playlist');
    const setSequentialStrategyBtn = document.getElementById('setSequentialStrategyBtn');
    const setRandomStrategyBtn = document.getElementById('setRandomStrategyBtn');
    // const strategyNameEl = document.getElementById('strategyName'); // Якщо потрібно динамічно змінювати

    const audioPlayer = document.getElementById('audioPlayer');
    const canvas = document.getElementById('soundWaveCanvas');
    const ctx = canvas.getContext('2d');

    let localFiles = {}; // Зберігає { 'назва файлу': File_object }
    let currentBackendState = 'Stopped';
    let currentBackendTrackName = null;
    let animationFrameId;

    // --- API функції ---
    async function apiCall(endpoint, method = 'GET', body = null) {
        const options = {
            method,
            headers: {}
        };
        if (body) {
            options.headers['Content-Type'] = 'application/json';
            options.body = JSON.stringify(body);
        }
        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
            if (!response.ok) {
                const errorText = await response.text();
                console.error(`API Error ${response.status}: ${errorText}`);
                alert(`Помилка API: ${errorText}`);
                return null;
            }
            if (response.headers.get("content-type")?.includes("application/json")) {
                return response.json();
            }
            return response.text();
        } catch (error) {
            console.error('Fetch error:', error);
            alert('Не вдалося з\'єднатися з сервером.');
            return null;
        }
    }

    // --- Оновлення UI ---
    async function updatePlayerStatus() {
        const status = await apiCall('/status');
        if (status) {
            currentBackendState = status.state;
            currentBackendTrackName = status.currentTrack;

            playerStateEl.textContent = status.state || 'N/A';
            currentTrackNameEl.textContent = status.currentTrack || '-';
            playPauseBtn.textContent = status.state === 'Playing' ? 'Pause' : 'Play';

            updateSoundWave(status.state);
            updatePlaylistHighlight();

            if (status.state === 'Playing' && status.currentTrack) {
                playLocalAudio(status.currentTrack);
            } else if (status.state === 'Paused') {
                audioPlayer.pause();
            } else if (status.state === 'Stopped') {
                audioPlayer.pause();
                audioPlayer.currentTime = 0;
            }
        }
        updateButtonsState();
    }

    async function updatePlaylist() {
        const playlistData = await apiCall('/getPlaylist');
        playlistEl.innerHTML = '';
        if (playlistData && Array.isArray(playlistData)) {
            playlistData.forEach(trackName => {
                const li = document.createElement('li');
                li.textContent = trackName;
                playlistEl.appendChild(li);
            });
        }
        updatePlaylistHighlight();
        updateButtonsState();
    }

    function updatePlaylistHighlight() {
        Array.from(playlistEl.children).forEach(li => {
            if (li.textContent === currentBackendTrackName && currentBackendState !== 'Stopped') {
                li.classList.add('playing');
            } else {
                li.classList.remove('playing');
            }
        });
    }

    function updateButtonsState() {
        const hasTracks = playlistEl.children.length > 0;
        playPauseBtn.disabled = !hasTracks;
        stopBtn.disabled = !hasTracks || currentBackendState === 'Stopped';
        nextBtn.disabled = !hasTracks;
        setFirstTrackBtn.disabled = !hasTracks;
    }


    // --- Керування відтворенням ---
    function playLocalAudio(trackName) {
        const file = localFiles[trackName];
        if (file) {
            if (audioPlayer.src !== URL.createObjectURL(file)) {
                if (audioPlayer.src && audioPlayer.src.startsWith('blob:')) {
                    URL.revokeObjectURL(audioPlayer.src);
                }
                audioPlayer.src = URL.createObjectURL(file);
            }
            audioPlayer.play().catch(e => console.error("Error playing audio:", e));
        } else {
            console.warn(`Локальний файл для треку "${trackName}" не знайдено.`);
        }
    }

    // --- Обробники подій ---
    playPauseBtn.addEventListener('click', async () => {
        if (currentBackendState === 'Playing') {
            await apiCall('/pause', 'GET');
        } else {
            await apiCall('/play', 'GET');
        }
        updatePlayerStatus();
    });

    stopBtn.addEventListener('click', async () => {
        await apiCall('/stop', 'GET');
        updatePlayerStatus();
    });

    nextBtn.addEventListener('click', async () => {
        await apiCall('/next', 'GET');
        updatePlayerStatus();
    });

    addSelectedFilesBtn.addEventListener('click', async () => {
        const files = fileInput.files;
        if (files.length === 0) {
            alert('Будь ласка, оберіть аудіофайли.');
            return;
        }

        let allTracksAddedSuccessfully = true;
        for (const file of files) {
            localFiles[file.name] = file; // Зберігаємо файл локально
            const result = await apiCall('/add-track', 'POST', file.name ); // Надсилаємо тільки назву
            if (!result || !result.message?.includes("Track added")) {
                allTracksAddedSuccessfully = false;
                console.warn(`Не вдалося додати трек ${file.name} на бекенд.`);
                // Видаляємо з localFiles, якщо бекенд не зміг його додати
                delete localFiles[file.name];
            }
        }
        fileInput.value = ''; // Очистити поле вводу файлів

        if (allTracksAddedSuccessfully) {
            alert('Обрані треки додано до плейлиста (назви відправлено на сервер).');
        } else {
            alert('Деякі треки не вдалося додати. Дивіться консоль для деталей.');
        }
        updatePlaylist();
        updatePlayerStatus(); // Оновити статус, якщо це вплинуло на поточний трек
    });

    setFirstTrackBtn.addEventListener('click', async () => {
        const result = await apiCall('/setFirstTrack', 'POST');
        if (result) {
            alert(result); // "Set first track in playlist"
            updatePlayerStatus();
        }
    });

    setSequentialStrategyBtn.addEventListener('click', async () => {
        const result = await apiCall('/strategy/sequntial', 'POST'); // Виправляємо sequntial
        if (result) {
            alert(result); // "Set sequential strategy"
            // Можна оновити strategyNameEl, якщо він є
        }
    });

    setRandomStrategyBtn.addEventListener('click', async () => {
        const result = await apiCall('/strategy/random', 'POST');
        if (result) {
            alert(result);
        }
    });

    audioPlayer.addEventListener('ended', async () => {
        // Коли трек закінчився, автоматично переходимо на наступний через бекенд
        console.log("Track ended, calling /next");
        await apiCall('/next', 'GET');
        updatePlayerStatus(); // Це має запустити наступний трек, якщо плейлист не закінчився і стан 'Playing'
    });

    audioPlayer.onpause = () => { // Коли користувач ставить на паузу через елемент <audio> (якщо він видимий)
        if (currentBackendState === 'Playing') { // Якщо бекенд думає, що грає
            // Синхронізувати стан з бекендом (опціонально, залежить від бажаної поведінки)
            // apiCall('/pause', 'GET').then(updatePlayerStatus);
            console.log("Audio paused locally, backend state might be Playing");
        }
        updateSoundWave(currentBackendState === 'Playing' ? 'PausedLocal' : currentBackendState); // Показати хвилю як на паузі
    };

    audioPlayer.onplay = () => { // Коли відтворення починається локально
         if (currentBackendState !== 'Playing') {
            // Синхронізувати стан з бекендом (опціонально)
            // apiCall('/play', 'GET').then(updatePlayerStatus);
            console.log("Audio playing locally, backend state might not be Playing");
        }
        updateSoundWave('Playing');
    };

    // --- Візуалізація звукової хвилі (симуляція) ---
    function drawSoundWave(state) {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.fillStyle = '#5a3e8c';

        if (state === 'Playing') {
            const barWidth = 4;
            const numBars = Math.floor(canvas.width / (barWidth + 1));
            for (let i = 0; i < numBars; i++) {
                const barHeight = Math.random() * (canvas.height - 10) + 5;
                ctx.fillRect(i * (barWidth + 1), canvas.height - barHeight, barWidth, barHeight);
            }
        } else if (state === 'Paused' || state === 'PausedLocal') {
            const barWidth = 4;
            const numBars = Math.floor(canvas.width / (barWidth + 1));
            for (let i = 0; i < numBars; i++) {
                const barHeight = (Math.sin(i * 0.3) * 0.3 + 0.4) * (canvas.height - 20) + 5; // Статична хвиля
                ctx.fillRect(i * (barWidth + 1), canvas.height - barHeight, barWidth, barHeight);
            }
        } else { // Stopped or other
            ctx.beginPath();
            ctx.moveTo(0, canvas.height / 2);
            ctx.lineTo(canvas.width, canvas.height / 2);
            ctx.strokeStyle = '#5a3e8c';
            ctx.lineWidth = 2;
            ctx.stroke();
        }
    }

    function updateSoundWave(playerState) {
        if (animationFrameId) {
            cancelAnimationFrame(animationFrameId);
        }

        function animateWave() {
            if (currentBackendState === 'Playing' && !audioPlayer.paused) { // Перевіряємо і локальний стан аудіо
                drawSoundWave('Playing');
                animationFrameId = requestAnimationFrame(animateWave);
            } else if (currentBackendState === 'Paused' || audioPlayer.paused) {
                drawSoundWave('Paused'); // Статична хвиля для паузи
            } else {
                 drawSoundWave('Stopped'); // Пряма лінія для зупинки
            }
        }

        if (playerState === 'Playing') {
            animateWave();
        } else if (playerState === 'Paused') {
            drawSoundWave('Paused');
        } else if (playerState === 'PausedLocal' && currentBackendState === 'Playing') { // Локальна пауза, коли бекенд думає, що грає
             drawSoundWave('Paused');
        }
        else {
            drawSoundWave('Stopped');
        }
    }


    // --- Ініціалізація ---
    async function initializeApp() {
        await updatePlayerStatus();
        await updatePlaylist();
        updateButtonsState();
    }

    initializeApp();
});