namespace WebAPISample.Models
{
    public class Class
    {
        public Class(int id, string? name, bool flg_data)
        {
            this.id = id;
            this.name = name;
            this.flg_data = flg_data;
        }

        public Class(int id, string? name)
        {
            this.id = id;
            this.name = name;
        }

        public Class()
        {
            id = 0;
            name = "noname";
            flg_data = false;
        }

        public int id { get; set; }
        public string? name { get; set; }
        public bool flg_data { get; set; }
    }
}
