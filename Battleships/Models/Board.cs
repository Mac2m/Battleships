namespace Battleships.Models
{
    public abstract class Board
    {
        public List<Field> Fields { get; set; }

        public Board()
        {
            Fields = new List<Field>();
            for (int i = 1; i <= 10; i++)
            {
                for (char j = 'A'; j <= 'J'; j++)
                {
                    Fields.Add(new Field(i, j));
                }
            }
        }
    }
}