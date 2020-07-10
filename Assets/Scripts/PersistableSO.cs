using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace garagekitgames
{
    //[CreateAssetMenu(menuName = "GarageKitGames/Singleton/PersistableSO")]
    public class PersistableSO : UnitySingletonPersistent<PersistableSO>
    {

        [Header("Meta")]
        public string persisterName;
        [Header("Scriptable Objects")]
        public List<ScriptableObject> objectsToPersist;

        public ScriptableObject version;

        public override void Awake()
        {
            base.Awake();

        }
       /* protected void OnEnable()
        {
            for (int i = 0; i < objectsToPersist.Count; i++)
            {
                if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name), FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objectsToPersist[i]);
                    file.Close();

                }
                else
                {
                    //Do Nothing
                }
            }
        }*/

        protected void OnDisable()
        {
            for (int i = 0; i < objectsToPersist.Count; i++)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name));
                var json = JsonUtility.ToJson(objectsToPersist[i]);
                bf.Serialize(file, json);
                file.Close();
            }

        }

        public void Save()
        {
            for (int i = 0; i < objectsToPersist.Count; i++)
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name));
                var json = JsonUtility.ToJson(objectsToPersist[i]);
                bf.Serialize(file, json);
                file.Close();
            }
        }
        public void Load()
        {
            for (int i = 0; i < objectsToPersist.Count; i++)
            {
                if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, objectsToPersist[i].name), FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objectsToPersist[i]);
                    file.Close();

                }
                else
                {
                    //Do Nothing
                }
            }
        }


        public void SaveVersion()
        {
            //for (int i = 0; i < objectsToPersist.Count; i++)
            //{
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, version.name));
                var json = JsonUtility.ToJson(version);
                bf.Serialize(file, json);
                file.Close();
            //
        }
        public void LoadVersion()
        {
            //for (int i = 0; i < objectsToPersist.Count; i++)
           // {
                if (File.Exists(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, version.name)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}_{1}.pso", persisterName, version.name), FileMode.Open);
                    JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), version);
                    file.Close();

                }
                else
                {
                    //Do Nothing
                }
            //}
        }
        // Use this for initialization
        /* void Start()
         {

         }

         // Update is called once per frame
         void Update()
         {

         }*/
    }
}

