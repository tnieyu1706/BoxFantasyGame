using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;

namespace TnieYuPackage.AvailablePackageExtensions
{
    [SingletonFactory]
    public class WaitForSecondsServiceLocator : ServiceLocatorManager<float, WaitForSeconds>
    {
        public override WaitForSeconds GetService(float key)
        {
            if (GetServiceLocator.ContainsKey(key))
            {
                return GetServiceLocator[key];
            }
            
            WaitForSeconds wait = new WaitForSeconds(key);
            GetServiceLocator.Add(key, wait);
            return wait;
        }
    }
}