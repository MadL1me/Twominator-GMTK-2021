using System.Collections.Generic;
using System.Linq;
using Levels;
using UnityEngine;

namespace LogicalElements
{
    public class LevelActivatablesController
    {
        public Dictionary<ColorEnum, List<ActivatorElement>> ColorToActivator =
            new Dictionary<ColorEnum, List<ActivatorElement>>();

        public Dictionary<ColorEnum, List<ListenerElement>> ColorToListener =
            new Dictionary<ColorEnum, List<ListenerElement>>();

        public LevelActivatablesController(GameLevel CurrentLevel, GameLevel PastLevel)
        {
            SynchronizeLevelsActivatableElements(CurrentLevel, PastLevel);
            SubscribeOnActivatorEvents();
        }

        private void SynchronizeLevelsActivatableElements(GameLevel CurrentLevel, GameLevel PastLevel)
        {
            var allActivators = new List<ActivatorElement>();
            var allListeners = new List<ListenerElement>();

            if (PastLevel != null)
            {
                allActivators.AddRange(PastLevel.GetLevelActivators.ToList());
                allListeners.AddRange(PastLevel.GetLevelListeners.ToList());   
            }

            if (CurrentLevel != null)
            {
                allActivators.AddRange(CurrentLevel?.GetLevelActivators);
                allListeners.AddRange(CurrentLevel?.GetLevelListeners);
            }

            var groupActivatorsByColorEnum = allActivators.GroupBy(activatable => activatable.ColorEnum);
            var groupListenersByColorEnum = allListeners.GroupBy(activatable => activatable.ColorEnum);

            ColorToListener = new Dictionary<ColorEnum, List<ListenerElement>>();
            ColorToActivator = new Dictionary<ColorEnum, List<ActivatorElement>>();

            foreach (var group in groupListenersByColorEnum)
            {
                if (!ColorToListener.ContainsKey(group.Key))
                {
                    ColorToListener[group.Key] = new List<ListenerElement>();
                }

                foreach (var activatable in group)
                {
                    ColorToListener[group.Key].Add(activatable);
                }
            }

            foreach (var colorGroup in groupActivatorsByColorEnum)
            {
                if (!ColorToActivator.ContainsKey(colorGroup.Key))
                {
                    ColorToActivator[colorGroup.Key] = new List<ActivatorElement>();
                }

                foreach (var activator in colorGroup)
                {
                    //activator.SetConnectedElements(ColorToListener[colorGroup.Key].ToArray());
                    ColorToActivator[colorGroup.Key].Add(activator);
                }
            }
        }

        private void OnActivatorSwitch(ColorEnum colorEnum)
        {
            Debug.Log("switch event occured");
            
            foreach (var listener in ColorToListener[colorEnum])
            {
                listener.Switch();
            }
        }

        private void OnActivatorActivate(ColorEnum colorEnum)
        {
            foreach (var listener in ColorToListener[colorEnum])
            {
                //listener.Activate();
            }
        }

        private void OnActivatorDeactivate(ColorEnum colorEnum)
        {
            foreach (var listener in ColorToListener[colorEnum])
            {
                //listener.Deactivate();
            }
        }

        private void SubscribeOnActivatorEvents()
        {
            foreach (var activatorsList in ColorToActivator)
            {
                foreach (var activatorElement in activatorsList.Value)
                {
                    activatorElement.OnActivate += OnActivatorActivate;
                    activatorElement.OnDeactivate += OnActivatorDeactivate;
                    activatorElement.OnSwitch += OnActivatorSwitch;
                }
            }
        }

        public void UnSubscribeFromActivatorEvents()
        {
            foreach (var activatorsList in ColorToActivator)
            {
                foreach (var activatorElement in activatorsList.Value)
                {
                    activatorElement.OnActivate -= OnActivatorActivate;
                    activatorElement.OnDeactivate -= OnActivatorDeactivate;
                    activatorElement.OnSwitch -= OnActivatorSwitch;
                }
            }
        }
    }
}