namespace DesignPatterns.Singleton.Command
{
    public class PlaceBuildingCommand : ICommand
    {
        private Building building;

        public PlaceBuildingCommand(Building building)
        {
            this.building = building;
        }
        
        public bool Execute()
        {
            building.gameObject.SetActive(true);
            return building.Place();
        }

        public void Undo()
        {
            building.Sell();
        }
    }
}