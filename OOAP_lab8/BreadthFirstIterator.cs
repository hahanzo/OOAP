using OOAP_lab8.Models;
using System.Collections;

namespace OOAP_lab8
{
    public class BreadthFirstIterator : IEnumerable<GeoNode>
    {
        private readonly GeoNode _root;

        public BreadthFirstIterator(GeoNode root)
        {
            _root = root;
        }

        public IEnumerator<GeoNode> GetEnumerator()
        {
            var queue = new Queue<GeoNode>();
            queue.Enqueue(_root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
