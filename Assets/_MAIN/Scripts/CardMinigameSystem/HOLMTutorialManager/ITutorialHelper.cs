using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TUTORIAL_MANAGER
{
    public interface ITutorialHelper
    {
        static event Action<string> TutorialHelper;
    }
}