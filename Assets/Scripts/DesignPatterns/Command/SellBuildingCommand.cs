namespace DesignPatterns.Singleton.Command
{
    public class SellBuildingCommand : ICommand
    {
        private Building building;

        public SellBuildingCommand(Building building)
        {
            this.building = building;
        }
        
        public bool Execute()
        {
            building.Remove();
            return true;
        }

        public void Undo()
        {
            building.gameObject.SetActive(true);
            building.Place();
        }
    }
}