using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;
using Object = System.Object;

namespace ResKits
{
    public class ResConsts
    {
        public const string ResIndexFile = "res";
        public const string ExtName = ".unity3d";                   //素材扩展名
    }
    
    public class AssetBundleData
    {
        public AssetBundle assetBundle;
        public int referencedCount;

        public AssetBundleData(AssetBundle assetBundle)
        {
            assetBundle = assetBundle;
            referencedCount = 1;
        }
    }

    class LoadAssetRequest
    {
        public Type assetType;
        public string[] assetNames;
        public Action<Object[]> callback;
    }

    class UnloadAssetBundleRequest
    {
        public string abName;
        public bool unloadNow;
        public AssetBundleData abData;
    }
    
    public class AssetBundleKit : MonoBehaviour
    {
        // private ResourceManager m_resMgr;
        private string[] m_allManifest = null;
        private AssetBundleManifest m_assetBundleManifest = null;
        /** 资源依赖 */
        Dictionary<string, string[]> m_dependencies = new Dictionary<string, string[]>();
        /** 加载中的 */
        List<string> m_assetBundleLoadingList = new List<string>();
        /** 已经加载的AB */
        Dictionary<string, AssetBundleData> m_loadedAssetBundles = new Dictionary<string, AssetBundleData>();
        /** 加载请求 */
        Dictionary<string, List<LoadAssetRequest>> m_loadRequests= new Dictionary<string, List<LoadAssetRequest>>();
        /** 释放请求 */
        Dictionary<string, UnloadAssetBundleRequest> m_assetBundleUnloadingDic = new Dictionary<string, UnloadAssetBundleRequest>();

        public Dictionary<string, AssetBundleData> LoadedAssetBundles => m_loadedAssetBundles;

        public AssetBundleKit(ResourceManager manager)
        {
            // m_resMgr = manager;
        }

        public void Initialize(string manifestName, Action callback)
        {
            LoadAsset(manifestName, new string[] { "AssetBundleManifest" }, typeof(AssetBundleManifest), delegate (Object[] objs)
            {
                if (objs.Length > 0)
                {
                    m_assetBundleManifest = objs[0] as AssetBundleManifest;
                    m_allManifest = m_assetBundleManifest.GetAllAssetBundles();
                }
                if (callback != null) callback();
            });
        }
        
        // 加载素材
        public void LoadAsset(string abName, string[] assetNames, Type assetType, Action<Object[]> callback = null)
        {
            abName = GetRealAssetPath(abName);
            var request = new LoadAssetRequest();
            request.assetType = assetType;
            request.assetNames = assetNames;
            request.callback = callback;

            List<LoadAssetRequest> requests = null;
            if (!m_loadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_loadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset(abName, assetType));
            }
            else
            {
                requests.Add(request);
            }
        }
        /// <summary>
        /// 加载Asset
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="assetType"></param>
        /// <returns></returns>
        IEnumerator OnLoadAsset(string abName, Type assetType)
        {
            AssetBundleData bundleData = GetLoadedAssetBundle(abName);
            if (bundleData == null)
            {
                yield return StartCoroutine(OnLoadAssetBundle(abName, assetType));

                bundleData = GetLoadedAssetBundle(abName);
                if (bundleData == null)
                {
                    m_loadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset-->>" + abName);
                    yield break;
                }
            }

            List<LoadAssetRequest> list = null;
            if (!m_loadRequests.TryGetValue(abName, out list))
            {
                m_loadRequests.Remove(abName);
            }

            for (int i = 0; i < list.Count; i++)
            {
                string[] assetNames = list[i].assetNames;
                List<Object> result = new List<Object>();

                AssetBundle ab = bundleData.assetBundle;
                if (assetNames != null)
                {
                    for (int j = 0; j < assetNames.Length; j++)
                    {
                        string assetPath = assetNames[i];
                        var request = ab.LoadAssetAsync(assetPath, assetType);
                        yield return request;
                        result.Add(request.asset);
                    }
                }
                else
                {
                    var request = ab.LoadAllAssetsAsync();
                    yield return request;
                    result = new List<Object>(request.allAssets);
                }

                if (list[i].callback != null)
                {
                    list[i].callback(result.ToArray());
                    list[i].callback = null;
                }

                bundleData.referencedCount++;
            }

            m_loadRequests.Remove(abName);
        }
        
        // 获取完整路径
        string GetAssetFullPath(string path)
        {
            string assetPath = Path.Combine(Application.streamingAssetsPath, ResConsts.ResIndexFile);
            return Path.Combine(assetPath, path);
        }
        
        // 获取真实Asset路径
        string GetRealAssetPath(string abName)
        {
            if (abName.Equals(ResConsts.ResIndexFile))
            {
                return abName;
            }

            abName = abName.ToLower();
            if (! abName.EndsWith(ResConsts.ExtName))
            {
                abName += ResConsts.ExtName;
            }

            if (abName.Contains("/"))
            {
                return abName;
            }

            for (int i = 0; i < m_allManifest.Length; i++)
            {
                int index = m_allManifest[i].LastIndexOf('/');
                string path = m_allManifest[i].Remove(0, index + 1);
                if (path.Equals(abName))
                {
                    return m_allManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }
        // 加载AssetBundle
        IEnumerator OnLoadAssetBundle(string abName, Type type)
        {
            string url = GetAssetFullPath(abName);
            if (!m_assetBundleLoadingList.Contains(url))
            {
                m_assetBundleLoadingList.Add(url);
            }

            var abUrl = Application.isEditor ? abName : url;
            Debug.Log(url);

            var request = AssetBundle.LoadFromFileAsync(url);
            if (abName != ResConsts.ResIndexFile)
            {
                string[] dependencies = m_assetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0)
                {
                    m_dependencies.Add(abName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        string depName = dependencies[i];
                        AssetBundleData bundleData = null;
                        if (m_loadedAssetBundles.TryGetValue(depName, out bundleData))
                        {
                            bundleData.referencedCount++;
                        } 
                        else if (!m_loadRequests.ContainsKey(depName))
                        {
                            // 加载 依赖的 ab 
                            yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                        }
                    }
                }
            }

            yield return request;

            AssetBundle assetBundle = request.assetBundle;
            if (assetBundle != null)
            {
                m_loadedAssetBundles.Add(abName, new AssetBundleData(assetBundle));
            }

            m_assetBundleLoadingList.Remove(url);
        }
        
        
        // 获取AssetBundle
        AssetBundleData GetLoadedAssetBundle(string abName)
        {
            AssetBundleData bundle = null;
            m_loadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;
            
            // No Dependencies are recorded , only the bundle itself is required
            string[] dependencies = null;
            if (!m_dependencies.TryGetValue(abName, out dependencies))
            {
                return bundle;
            }
            
            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                AssetBundleData dependenBundle;
                m_loadedAssetBundles.TryGetValue(dependency, out dependenBundle);
                if (dependenBundle == null) return null;
            }

            return bundle;
        }

        public void Update()
        {
            this.DealWithUnloadRequest();
        }
        
        /** 处理UnloadRequest */
        private void DealWithUnloadRequest()
        {
            if (m_assetBundleUnloadingDic.Count == 0) return;
            foreach (var pair in m_assetBundleUnloadingDic)
            {
                // 如果在加载List中则忽略
                if (m_assetBundleLoadingList.Contains(pair.Key)) continue;
                var request = pair.Value;

                if (request.abData != null && request.abData.assetBundle != null)
                {
                    request.abData.assetBundle.Unload(true);
                }

                m_assetBundleLoadingList.Remove(pair.Key);
                m_loadedAssetBundles.Remove(pair.Key);
                Debug.Log(pair.Key + " has been unloaded successfully");
            }
        }
    }
}

