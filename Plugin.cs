using BepInEx;
using DG.Tweening;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoMoreDoors
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        GameObject frontDoor;
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Logger.LogInfo("Removing Doors");
            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            SceneManager.sceneLoaded += (a, b) => frontDoor = null;
        }
        private void Update()
        {
            if (frontDoor == null)
            {
                frontDoor = GameObject.FindObjectsOfType<GameObject>().FirstOrDefault(x => x.name == "Showcase_Store_Door_05" && x.transform.parent.parent.gameObject.name == "--- To be enabled ---  ");
                if (frontDoor != null) frontDoor.SetActive(false);
            }
        }
    }
    public static class DoorsPatch
    {
        [HarmonyPatch(typeof(DoorToStore), "Start")]
        public static class DoorToStore_Start_Patch
        {
            public static void Postfix(DoorToStore __instance) => __instance.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        [HarmonyPatch(typeof(StorageDoor), "Start")]
        public static class StorageDoor_Start_Patch
        {
            public static void Postfix(StorageDoor __instance) => __instance.gameObject.SetActive(false);
        }
    }
}
