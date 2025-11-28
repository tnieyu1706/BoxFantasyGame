using System;
using _Project.Scripts.LoggingSystem;
using JetBrains.Annotations;
using Systems.GameAction.Editor;
using Systems.IdentifySystem.ObjectIdentify;
using Systems.IdentifySystem.ObjectIdentify.StaticIdentify;
using Systems.IdentifySystem.TypeIdentify;
using TnieYuPackage.DesignPatterns.Patterns.Builder;
using UnityEngine;

namespace Systems.GameAction
{
    [Serializable]
    public class GameActionCommand
    {
        [SerializeReference] public IObjectIdentify sender;
        [SerializeReference] public IObjectIdentify target;
        [SerializeReference] public IGameAction action;

        private GameActionCommandStaticDto dtoTemp;

        public GameActionCommand(IObjectIdentify sender, IObjectIdentify target, [NotNull] IGameAction action)
        {
            this.sender = sender;
            this.target = target;
            this.action = action;
        }

        public void Trigger()
        {
            action.Execute(sender, target);
            GameLogger.Instance?.Log(
                action.GetIdentify(),
                $"{action.GetIdentify()}: {sender?.ObjectName ?? "None"} -> {target?.ObjectName ?? "None"}"
            );

            //sau & neu co the thi check luon la co dc check hay ko ? (Execute -> bool)

            if (ActionIdentifyStorage.Instance.TryToGetLoadEvent(action.GetIdentify(), out var triggerAction))
            {
                ConvertToDto(out dtoTemp);
                if (dtoTemp == null)
                {
                    Debug.LogWarning("action is not valid.");
                    return;
                }

                triggerAction.Invoke(dtoTemp);
            }
            else
            {
                Debug.Log("don't find action identify");
            }
        }

        public string GetActionName()
        {
            if (action == null) return string.Empty;

            return ActionIdentifyStorage.Instance.GetIdByType(action.GetType());
        }

        public void ConvertToDto([CanBeNull] out GameActionCommandStaticDto dto)
        {
            string actionName = GetActionName();
            if (string.IsNullOrEmpty(actionName))
            {
                Debug.LogError(actionName + " action could not be found in ActionStorage.");
                dto = null;
                return;
            }

            dto = new GameActionCommandStaticDto()
            {
                actionName = actionName
            };

            if (sender is StaticObjectIdentify senderStatic)
                dto.sender = senderStatic;

            if (target is StaticObjectIdentify targetStatic)
                dto.target = targetStatic;
        }

        public static bool Matching(GameActionCommand source, GameActionCommand target)
        {
            if (source.action.GetIdentify() != target.action.GetIdentify())
                return false;

            bool isSenderMatching = true;
            bool isTargetMatching = true;

            if (target.sender != null)
            {
                isSenderMatching = source.sender != null && source.sender.Id.Equals(target.sender.Id);
            }

            if (target.target != null)
            {
                isTargetMatching = source.target != null && source.target.Id.Equals(target.target.Id);
            }

            return isSenderMatching && isTargetMatching;
        }

        public static bool Matching(GameActionCommand source, GameActionCommandStaticDto target)
        {
            if (source.action.GetIdentify() != target.actionName)
                return false;

            bool isSenderMatching = true;
            bool isTargetMatching = true;

            if (target.sender != null)
            {
                isSenderMatching = source.sender != null && source.sender.Id.Equals(target.sender.Id);
            }

            if (target.target != null)
            {
                isTargetMatching = source.target != null && source.target.Id.Equals(target.target.Id);
            }

            return isSenderMatching && isTargetMatching;
        }
    }

    [Serializable]
    public class GameActionCommandStaticDto : IMyBuilder<GameActionCommand>
    {
        public StaticObjectIdentify sender;
        public StaticObjectIdentify target;
        [SelectAction] public string actionName;

        public GameActionCommand Build()
        {
            ActionIdentifyStorage.Instance.Datas.Dictionary.TryGetValue(actionName, out string actionTypeName);
            if (string.IsNullOrEmpty(actionTypeName))
            {
                Debug.LogWarning("actionName is invalid");
                return null;
            }

            Type actionType = Type.GetType(actionTypeName);

            if (actionType != null && typeof(IGameAction).IsAssignableFrom(actionType))
            {
                return new GameActionCommand(
                    sender,
                    target,
                    (IGameAction)Activator.CreateInstance(actionType)
                );
            }

            Debug.LogWarning("actionType is invalid");
            return null;
        }

        /// <summary>
        /// Check for source & target is matching data
        /// when target sender, target not found => true.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Matching(GameActionCommandStaticDto source, GameActionCommandStaticDto target)
        {
            if (source.actionName != target.actionName)
                return false;

            bool isSenderMatching = true;
            bool isTargetMatching = true;

            if (target.sender != null)
            {
                isSenderMatching = source.sender != null && source.sender.Id.Equals(target.sender.Id);
            }

            if (target.target != null)
            {
                isTargetMatching = source.target != null && source.target.Id.Equals(target.target.Id);
            }

            return isSenderMatching && isTargetMatching;
        }

        /// <summary>
        /// Check for source & target is matching data
        /// when target sender, target not found => true.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool Matching(GameActionCommandStaticDto source, GameActionCommand target)
        {
            if (source.actionName != target.GetActionName())
                return false;

            bool isSenderMatching = false;
            bool isTargetMatching = false;

            if (target.sender != null)
            {
                isSenderMatching = source.sender != null && source.sender.Id.Equals(target.sender.Id);
            }

            if (target.target != null)
            {
                isTargetMatching = source.target != null && source.target.Id.Equals(target.target.Id);
            }

            return isSenderMatching && isTargetMatching;
        }
    }
}