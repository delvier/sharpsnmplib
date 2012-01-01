namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SelectionType : ISmiType
    {
        public SelectionType(string name, ISmiType subtype)
        {
            Name = name;
            Subtype = subtype;
        }

        public string Name { get; set; }
        public ISmiType Subtype { get; set; }
    }
}