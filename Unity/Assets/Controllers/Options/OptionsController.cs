using System;

namespace Assets.Controllers.Options
{
    public class OptionsController
    {
        private readonly Action<IMainControllerOptions> _resetMain;
        
        private IMainControllerOptions _options;

        public OptionsController(Action<IMainControllerOptions> resetMain, IMainControllerOptions initialOptions)
        {
            _resetMain = resetMain;
            _options = initialOptions;
        }
    }
}
