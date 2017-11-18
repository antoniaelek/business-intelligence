namespace DWClient.Models
{
    public class DatabaseObject<T>
    {
        public string SqlName { get; set; }
        public T Value { get; set; }

        public DatabaseObject(string sqlName)
        {
            SqlName = sqlName;
        }

        public DatabaseObject(string sqlName, T value)
        {
            SqlName = sqlName;
            Value = value;
        }
    }
}
