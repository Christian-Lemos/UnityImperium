namespace Imperium.Research
{
    [System.Serializable]
    public class ResearchNode
    {
        public Research research;
        public ResearchEra reserachEra;
        public bool completed;

        public ResearchNode(Research research, ResearchEra reserachEra, bool completed)
        {
            this.research = research;
            this.reserachEra = reserachEra;
            this.completed = completed;
        }
    }
}