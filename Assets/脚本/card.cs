public class Card
{
    public int id;
    public string name;
    public Card(int _id,string _name)
    {
        this.id=_id;
        this.name=_name;
    }
}

public class shoupai:Card
{
    public int damageW;
    public int damageC;
    public int damageZ;
    public int damageG;
    public shoupai(int _id,string _name,int _damageW,int _damageC,int _damageZ,int _damageG):base(_id,_name)
    {
        this.damageC=_damageC;
        this.damageG=_damageG;
        this.damageW=_damageW;
        this.damageZ=_damageZ;
    }
}