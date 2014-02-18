using System;
using Assets.Controllers.Level;

namespace Assets.Controllers.Options
{
    public class OptionsController
    {
        private readonly Action<ILevelControllerOptions> _resetMain;
        
        public ILevelControllerOptions Options;

        public OptionsController(Action<ILevelControllerOptions> resetMain)
        {
            _resetMain = resetMain;
            Options = InitialOptionsFactory.Build();

        }
    }
}
