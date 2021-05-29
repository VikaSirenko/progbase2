using System.Xml.Serialization;

[XmlType(TypeName = "platform")]
public class Platform
{
    public long id;
    public string name;
    public Platform()
    {
        this.id = default;
        this.name = default;
    }

    public override string ToString()
    {
        return $"[{id}] | Game:'{name}' ";
    }
}
