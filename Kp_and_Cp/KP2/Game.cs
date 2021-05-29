using System.Collections.Generic;
using System.Xml.Serialization;


[XmlType(TypeName = "platform")]
public class Game
{
    public long id;
    public string name;
    public int year;
    public double globalSales;
    public List<Platform> platform;

    public Game()
    {
        this.id = default;
        this.name = default;
        this.year = default;
        this.globalSales = default;

    }
    public override string ToString()
    {
        return $"[{id}] | Game:'{name}' | Year: '{year}'";
    }



}
