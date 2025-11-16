using System.Collections.Generic;
using System.Linq;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using TnieYuPackage.Utils;

namespace Systems.IdentifySystem.ObjectIdentify
{
    public abstract class BaseCategoryIdentifyManager<TCategory, TObject, TManager> : SingletonScriptable<TManager>
        where TCategory : ICategoryIdentify
        where TManager : BaseCategoryIdentifyManager<TCategory, TObject, TManager>
    {
        public List<TCategory> categoryIdentifies = new();

        public Dictionary<string, TCategory> categoryMap
        {
            get => categoryIdentifies.ToDictionary(x => x.CategoryName);
        }

        public TCategory GetCategoryByCategoryIdentify(string categoryIdentify)
        {
            categoryMap.TryGetValue(categoryIdentify, out TCategory category);
            return category;
        }

        public abstract TObject QuickGetObjectById(string categoryIdentify, SerializableGuid objectId);
        public abstract TObject QuickGetObjectByName(string categoryIdentify, string objectName);
    }
}