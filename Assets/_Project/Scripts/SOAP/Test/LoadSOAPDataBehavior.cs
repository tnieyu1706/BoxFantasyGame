using AYellowpaper;
using EditorAttributes;
using Systems.SOAP.Data;
using UnityEngine;

namespace _Project.Scripts.SOAP.Test
{
    public class LoadSOAPDataBehavior : MonoBehaviour
    {
        [PropertyDropdown]
        public SOAPFloatData soapFloatData;

        [TypeDropdown]
        public string Type;
    }
}