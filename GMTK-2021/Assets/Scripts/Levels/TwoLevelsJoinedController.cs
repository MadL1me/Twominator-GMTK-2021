using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class TwoLevelsJoinedController : MonoBehaviour
    {
        private List<GameLevel> _levels; 
        
        private GameLevel _pastLevel;
        private GameLevel _futureLevel;
    }
}