using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class manejador_gameplay : MonoBehaviour
{
    [SerializeField] private AudioSource[] a_s;
    [SerializeField] private AudioClip[] clips1;
    [SerializeField] private GameObject[] ts;

    public string dataPath = "";
    public string[] dataFileName;


    public TMP_Text[] texto;
    public float segundos = 0;
    public bool aumentando = false,post;
    public bool on = false,firstlevel,UseEncryption;
    public string palabra_clave;
    public int enemigos_derrotados = 0;
    public int enemigos_nivel;
    public int levelnum = 0;

    public GameData GD;
    public GameData[] GL = new GameData[2];
    public LevelData[] LL = new LevelData[2];
    public GameObject[] enemies;

    public bool[] istheredata;
    bool DLoaded;

    public int[] levels;
    public string[] levelsoutcomes;
    public string[] levelstimes;

    public void OnEnable()
    {
        a_s[0].Play();
        on = true;
        if (LL[levelnum-1].enemy_quantity == 0)
        {
            for(int i = 0; i < 4; i++)
            {
                Destroy(enemies[i]);
            }
        }
        if (LL[levelnum - 1].enemy_quantity == 1)
        {            
        }
        if (LL[levelnum - 1].enemy_quantity == 2)
        {
            for (int i = 3; i <= 9; i++)
            {
                if(enemies[i] != null)
                enemies[i].SetActive(true);
            }
        }
    }

    public void Start()
    {
        if (post)
        {
            a_s[0].Play();
            if (LL[levelnum - 1].enemy_quantity == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Destroy(enemies[i]);
                }
            }
            if (LL[levelnum - 1].enemy_quantity == 1)
            {
            }
            if (LL[levelnum - 1].enemy_quantity == 2)
            {
                for (int i = 3; i <= 9; i++)
                {
                    if (enemies[i] != null)
                        enemies[i].SetActive(true);
                }
            }
        }
    }

    public void Awake()
    {
        GD = new GameData();
        dataPath = Application.persistentDataPath + "levels";
        if (firstlevel)
        {
            GL[0] = Load();
            if (dataFileName[1] != "")
                GL[1] = Load();
        }
        if (GL[0] != null)
        {
            istheredata[0] = true;
            levels[0] = GL[0].level_number;
            levelsoutcomes[0] = GL[0].level_outcome;
            levelstimes[0] = GL[0].level_time; 
        }
        if (GL[1] != null)
        {
            istheredata[1] = true;
            levels[1] = GL[1].level_number;
            levelsoutcomes[1] = GL[1].level_outcome;
            levelstimes[1] = GL[1].level_time; 
        }
        LL[0] = LLoad();
        LL[1] = LLoad();
        if (LL[0] == null)
            Save(new LevelData(), dataFileName[2]);
        if (LL[1] == null)
            Save(new LevelData(), dataFileName[3]);
        GL[0] = Load();
        GL[1] = Load();
    }

    void Update()
    {
        if (!on) return;
        if (aumentando == false && on == true)
        {
            aumentando = true;
            segundos += Time.deltaTime;
            float minutos = Mathf.FloorToInt(segundos / 60);
            float displaysegundos = Mathf.FloorToInt(segundos % 60);
            texto[0].text = (minutos.ToString() + ":" + displaysegundos.ToString());
            aumentando = false;
        }        
    }

    public void gameend()
    {
        a_s[0].Stop();
        ts[0].GetComponent<movimientos_ui>().salida();
        ts[1].SetActive(true);
        ts[2].SetActive(true);

        if(enemigos_derrotados == enemigos_nivel)
        {
            texto[2].text = ("GUERRA");
        }
        if (enemigos_derrotados != enemigos_nivel && enemigos_derrotados < 0)
        {
            texto[2].text = ("NEUTRAL");
        }
        if (enemigos_derrotados == 0)
        {
            texto[2].text = ("PAZ");
        }

        float minutos = Mathf.FloorToInt(segundos / 60);
        float displaysegundos = Mathf.FloorToInt(segundos % 60);
        texto[0].text = (minutos.ToString() + ":" + displaysegundos.ToString());
        texto[1].text = texto[0].text;

        string[] unos;
        string[] doses;
        if (istheredata[0])
        {
            //unos = GL[0].level_time.Split(char.Parse(":"));
            unos = levelstimes[0].Split(char.Parse(":"));
            doses = texto[1].text.Split(char.Parse(":"));
            if (int.Parse(unos[0]) > int.Parse(doses[0]))
            {
                GD.level_number = levelnum;
                GD.level_outcome = texto[2].text;
                GD.level_time = texto[1].text;
                return;
            }
            if (int.Parse(unos[0]) == int.Parse(doses[0])) {
                if (int.Parse(unos[1]) >= int.Parse(doses[1]))
                {
                    GD.level_number = levelnum;
                    GD.level_outcome = texto[2].text;
                    GD.level_time = texto[1].text;
                    return;
                }
                else
                {
                    GD.level_number = levels[0];
                    GD.level_outcome = levelsoutcomes[0];
                    GD.level_time = levelstimes[0];
                }
            }
            else
            {
                GD.level_number = levels[0];
                GD.level_outcome = levelsoutcomes[0];
                GD.level_time = levelstimes[0];
            }
        }
        else
        {
            GD.level_number = levelnum;
            GD.level_outcome = texto[2].text;
            GD.level_time = texto[1].text;
        }
        //Save(GD);        
    }

    public void Save(GameData data, string dfn)
    {
        string fullpath = Path.Combine(dataPath, dfn);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (UseEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ocurrió un error al momento de guardar: " + fullpath + "\n" + e);
        }
    }

    public void Save(LevelData data, string dfn)
    {
        string fullpath = Path.Combine(dataPath, dfn);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (UseEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ocurrió un error al momento de guardar: " + fullpath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullpath;
        if (!DLoaded)
        {
            fullpath = Path.Combine(dataPath, dataFileName[0]);
            DLoaded = true;
        }
        else
            fullpath = Path.Combine(dataPath, dataFileName[1]);

        GameData loadedData = null;
        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (UseEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Ocurrió un error al momento de cargar: " + fullpath + "\n" + e);
            }            
        }
        return loadedData;
    }

    public LevelData LLoad()
    {
        string fullpath;
        if (dataFileName[1] != "")
            fullpath = Path.Combine(dataPath, dataFileName[1]);
        else
            fullpath = Path.Combine(dataPath, dataFileName[0]);
        if (dataFileName[2] != "")
        {
            fullpath = Path.Combine(dataPath, dataFileName[2]);
            if (dataFileName[3] != "")
                fullpath = Path.Combine(dataPath, dataFileName[3]);
        }

        LevelData loadedData = null;
        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (UseEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<LevelData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Ocurrió un error al momento de cargar: " + fullpath + "\n" + e);
            }
        }
        return loadedData;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i< data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ palabra_clave[i % palabra_clave.Length]);
        }
        return modifiedData;
    }
}

public class GameData
{
    public int level_number;

    public string level_outcome;

    public string level_time;

    public GameData()
    {
        this.level_number = 0;
        this.level_outcome = "n";
        this.level_outcome = "0:0";
    }
}

public class LevelData
{
    public int level_number;

    public int enemy_quantity;

    public LevelData()
    {
        this.level_number = 1;
        this.enemy_quantity = 1;

    }
}
