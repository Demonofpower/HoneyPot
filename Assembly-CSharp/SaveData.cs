using System;
using System.Runtime.Serialization;

// Token: 0x02000135 RID: 309
[Serializable]
public class SaveData : ISerializable
{
	// Token: 0x0600074B RID: 1867 RVA: 0x00036640 File Offset: 0x00034840
	public SaveData(int saveFileCount)
	{
		this.settingsMusicVol = 10;
		this.settingsSoundVol = 10;
		this.settingsVoiceVol = 10;
		this.settingsVoice = 0;
		this.settingsScreenFull = false;
		this.settingsScreenAspect = false;
		this.settingsScreenSize = -1;
		this.settingsLimit = 0;
		this.settingsCensored = true;
		this._saveFiles = new SaveFile[saveFileCount];
		for (int i = 0; i < this._saveFiles.Length; i++)
		{
			this._saveFiles[i] = new SaveFile();
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x000366C8 File Offset: 0x000348C8
	public SaveData(SerializationInfo info, StreamingContext context)
	{
		this.settingsMusicVol = (int)info.GetValue("settingsMusicVol", typeof(int));
		this.settingsSoundVol = (int)info.GetValue("settingsSoundVol", typeof(int));
		this.settingsVoiceVol = (int)info.GetValue("settingsVoiceVol", typeof(int));
		this.settingsVoice = (int)info.GetValue("settingsVoice", typeof(int));
		this.settingsScreenFull = (bool)info.GetValue("settingsScreenFull", typeof(bool));
		this.settingsScreenAspect = (bool)info.GetValue("settingsScreenAspect", typeof(bool));
		this.settingsScreenSize = (int)info.GetValue("settingsScreenSize", typeof(int));
		this.settingsLimit = (int)info.GetValue("settingsLimit", typeof(int));
		this.settingsCensored = (bool)info.GetValue("settingsCensored", typeof(bool));
		this._saveFiles = (SaveFile[])info.GetValue("saveFiles", typeof(SaveFile[]));
		for (int i = 0; i < this._saveFiles.Length; i++)
		{
			this._saveFiles[i].VerifyFile();
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00036844 File Offset: 0x00034A44
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("settingsMusicVol", this.settingsMusicVol);
		info.AddValue("settingsSoundVol", this.settingsSoundVol);
		info.AddValue("settingsVoiceVol", this.settingsVoiceVol);
		info.AddValue("settingsVoice", this.settingsVoice);
		info.AddValue("settingsScreenFull", this.settingsScreenFull);
		info.AddValue("settingsScreenAspect", this.settingsScreenAspect);
		info.AddValue("settingsScreenSize", this.settingsScreenSize);
		info.AddValue("settingsLimit", this.settingsLimit);
		info.AddValue("settingsCensored", this.settingsCensored);
		info.AddValue("saveFiles", this._saveFiles);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x000078D0 File Offset: 0x00005AD0
	public SaveFile GetSaveFile(int index)
	{
		return this._saveFiles[index];
	}

	// Token: 0x04000864 RID: 2148
	public int settingsMusicVol;

	// Token: 0x04000865 RID: 2149
	public int settingsSoundVol;

	// Token: 0x04000866 RID: 2150
	public int settingsVoiceVol;

	// Token: 0x04000867 RID: 2151
	public int settingsVoice;

	// Token: 0x04000868 RID: 2152
	public bool settingsScreenFull;

	// Token: 0x04000869 RID: 2153
	public bool settingsScreenAspect;

	// Token: 0x0400086A RID: 2154
	public int settingsScreenSize;

	// Token: 0x0400086B RID: 2155
	public int settingsLimit;

	// Token: 0x0400086C RID: 2156
	public bool settingsCensored;

	// Token: 0x0400086D RID: 2157
	private SaveFile[] _saveFiles;
}
