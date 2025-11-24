using System;
using _Project.Scripts.LoggingSystem;
using UnityEngine;

namespace _Project.Test
{
    public class TestGameLogger : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameLogger.Instance.Log("key1", "hello xin chao ngay moi tot lanh");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameLogger.Instance.Log("key1", "khong biet nua");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameLogger.Instance.Log("key2", "nihao moi nguoi");
            }
        }
    }
}