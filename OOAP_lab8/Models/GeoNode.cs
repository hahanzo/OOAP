namespace OOAP_lab8.Models
{
    public class GeoNode
    {
        public string Name { get; set; }
        public List<GeoNode> Children { get; set; } = new List<GeoNode>();

        public GeoNode(string name)
        {
            Name = name;
        }

        public void AddChild(GeoNode child)
        {
            Children.Add(child);
        }
    }
}
