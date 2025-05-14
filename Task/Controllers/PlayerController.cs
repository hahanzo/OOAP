using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Observer;
using Task.Services;
using Task.Strategy;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private static readonly PlayerContext _player = new();

        static PlayerController() 
        {
            _player.Attach(new ConsoleLogger());
        }

        [HttpGet("play")]
        public IActionResult Play()
        {
            _player.Play();
            return Ok(new { state = _player.StateName });
        }

        [HttpGet("pause")]
        public IActionResult Pause()
        {
            _player.Pause();
            return Ok(new { state = _player.StateName });
        }

        [HttpGet("stop")]
        public IActionResult Stop() 
        {
            _player.Stop();
            return Ok(new { state = _player.StateName });
        }

        [HttpGet("status")]
        public IActionResult Status() 
        {
            return Ok(new { state = _player.StateName, currentTrack = _player.CurrentTrack });
        }

        [HttpGet("next")]
        public IActionResult Next()
        {
            var track = _player.NextTrack();
            return Ok(track);
        }

        [HttpGet("getPlaylist")]
        public IActionResult GetPlaylist()
        {
            var playlist = _player.GetPlaylist();
            return Ok(playlist);
        }

        [HttpPost("setFirstTrack")]
        public IActionResult SetTrack()
        {
            _player.SetFirstTrack();
            return Ok("Set first track in playlist");
        }

        [HttpPost("add-track")]
        public IActionResult AddTrack([FromBody] string track)
        {
            if (string.IsNullOrWhiteSpace(track))
                return BadRequest("Track can't be empty");

            _player.AddTrack(track);
            return Ok(new { message = "Track added", track });
        }

        [HttpPost("strategy/sequntial")]
        public IActionResult UseSequntialStrategy()
        {
            _player.SetStrategy(new SequentialStrategy());
            return Ok("Set sequential strategy");
        }
    }
}
