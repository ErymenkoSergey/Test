namespace Test.LavaProject.Farm.UiInterface
{
    interface IClickable
    {
        public void Click(out int index, out GrowthStatus status, out PlantType type);
    }
}