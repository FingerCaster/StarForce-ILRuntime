namespace UGFExtensions.Hotfix
{
    public partial class HotfixGameEntry
    {
        public HPBarComponent HpBar { get; private set; }

        private void InitCustomComponents()
        {
            HpBar = new HPBarComponent();
            HpBar.Initialize();
            UpdateEvent += HpBar.Update;
        }

        private void ShutDownCustomComponents()
        {
            UpdateEvent -= HpBar.Update;
        }
    }
}