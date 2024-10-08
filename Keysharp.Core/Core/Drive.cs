namespace Keysharp.Core
{
	public static class Drive
	{
		//private static Keysharp.Core.Common.Drive
		/// <summary>
		/// Ejects the specified CD drive.
		/// </summary>
		/// <param name="path">Path of the CD drive to eject.</param>
		public static void DriveEject(object obj) => DriveHelper(obj.As(), true);

		/// <summary>
		/// Retrieves the capacity of a drive, in megabytes.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The capacity of the drive in megabytes</returns>
		public static long DriveGetCapacity(object obj) => new DriveInfo(obj.As()).TotalSize / 1024 / 1024;

		/// <summary>
		/// Retrieves the file system name of a drive.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The name of the file system of the drive</returns>
		public static string DriveGetFileSystem(object obj) => new DriveInfo(obj.As()).DriveFormat;

		/// <summary>
		/// Retrieves the label of a drive.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The label of the drive</returns>
		public static string DriveGetLabel(object obj) => new DriveInfo(obj.As()).VolumeLabel;

		/// <summary>
		/// Returns a string of letters, one character for each drive letter in the system.
		/// </summary>
		/// <param name="type">If omitted, all drive types are retrieved. Otherwise, specify one of the following words to retrieve only a specific type of drive: CDROM, REMOVABLE, FIXED, NETWORK, RAMDISK, UNKNOWN.</param>
		/// <returns>The drive ltters of the system</returns>
		public static string DriveGetList(object obj = null)
		{
			var drivetype = obj.As();
			var matchingDevices = "";
			DriveType? type = null;

			if (!string.IsNullOrEmpty(drivetype))
				type = Common.Mapper.MappingService.Instance.DriveType.LookUpCLRType(drivetype);

			var drives = DriveInfo.GetDrives();
#if !WINDOWS
			var list = new List<string>(drives.Length);
#endif
			for (var i = 0; i < drives.Length; i++)
			{
				if (type.HasValue)
				{
					if (i == 0) continue; // performance hack: skip A:\\

					try
					{
						if (drives[i].DriveType == type.Value)
#if WINDOWS
							matchingDevices += drives[i].Name[0];
#else
							list.Add(drives[i].Name);
#endif
					}
					catch
					{
						// ignore
					}
				}
				else
				{
#if WINDOWS
							matchingDevices += drives[i].Name[0];
#else
							list.Add(drives[i].Name);
#endif
				}
			}
#if WINDOWS
			return matchingDevices;
#else
			return string.Join(';', list);
#endif
		}

		/// <summary>
		/// Retrieves the serial number of a drive.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The serial number of the drive</returns>
		public static long DriveGetSerial(object obj)
		=> Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(obj.As())).Serial;

		/// <summary>
		/// Retrieves the free disk space of a drive, in megabytes.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The free drive space in megabytes</returns>
		public static long DriveGetSpaceFree(object obj) => new DriveInfo(obj.As()).TotalFreeSpace / (1024 * 1024);

		/// <summary>
		/// Retrieves the status of a drive.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The status of the drive</returns>
		public static string DriveGetStatus(object obj)
		{
			var drv = new DriveInfo(obj.As().TrimEnd('\\'));
			var val = drv.DriveFormat;//Will throw DriveNotFoundException on invalid paths.
			return drv.IsReady ? "Ready" : "NotReady";
		}

		/// <summary>
		/// Retrieves the status of a CD drive
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The status of the CD drive</returns>
		public static string DriveGetStatusCD(object obj)
		{
			var drive = Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(obj.As().TrimEnd('\\')));
			return drive.StatusCD;
		}

		/// <summary>
		/// Retrieves the type of a drive.
		/// </summary>
		/// <param name="path">Path of drive to receive information from.</param>
		/// <returns>The type of the drive</returns>
		public static string DriveGetType(object obj) => Common.Mapper.MappingService.Instance.DriveType.LookUpKeysharpType(new DriveInfo(obj.As()).DriveType);

		/// <summary>
		/// Prevents the eject feature of the specified drive from working.
		/// </summary>
		/// <param name="path">Path of drive to lock.</param>
		public static void DriveLock(object obj) => Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(obj.As())).Lock();

		/// <summary>
		/// Retracts the specified CD drive.
		/// </summary>
		/// <param name="path">Path of the CD drive to retract.</param>
		public static void DriveRetract(object obj) => DriveHelper(obj.As(), false);

		/// <summary>
		/// Sets the label of a drive. This needs administrator privileges to run.
		/// </summary>
		/// <param name="path">Path of drive to write information to.</param>
		/// <param name="label">The label to set the drive to.</param>
		public static void DriveSetLabel(object obj0, object obj1 = null)
		{
			var drv = obj0.As();
			var label = obj1.As();
			var di = new DriveInfo(drv);
			var drive = Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(di);
			drive.VolumeLabel = string.IsNullOrEmpty(label) ? "" : label;
		}

		/// <summary>
		/// Restores the eject feature of the specified drive.
		/// </summary>
		/// <param name="path">Path of drive to unlock.</param>
		public static void DriveUnlock(object obj) => Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(obj.As())).UnLock();

		/// <summary>
		/// adapted from http://stackoverflow.com/questions/398518/how-to-implement-glob-in-c
		/// </summary>
		/// <param name="glob"></param>
		/// <returns></returns>
		internal static IEnumerable<string> Glob(string glob)
		{
			if (System.IO.File.Exists(glob) || Directory.Exists(glob))
			{
				yield return glob;
				yield break;
			}

			foreach (var path in Glob(Dir.PathHead(glob) + Path.DirectorySeparatorChar, Dir.PathTail(glob)))
			{
				yield return path;
			}
		}

		internal static IEnumerable<string> Glob(string head, string tail)
		{
			if (Dir.PathTail(tail) == tail)
			{
				foreach (var path in Directory.GetFiles(head, tail))
				{
					yield return path;
				}
			}
			else
			{
				foreach (var dir in Directory.GetDirectories(head, Dir.PathHead(tail)))
				{
					foreach (var path in Glob(Path.Combine(head, dir), Dir.PathTail(tail)))
					{
						yield return path;
					}
				}
			}
		}

		private static void DriveHelper(string dr, bool b)
		{
			Keysharp.Core.Common.DriveBase drive;

			if (dr.Length == 0)
			{
				var allDrives = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.CDRom).ToList();
				drive = allDrives.Count > 0
						? Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(allDrives[0].Name))
						: throw new Error("Failed to find any CDROM or DVD drives.");
			}
			else
				drive = Keysharp.Core.Common.Platform.DriveProvider.CreateDrive(new DriveInfo(dr));

			if (b)
				drive.Eject();
			else
				drive.Retract();
		}
	}

	public class ShortcutOutput
	{
		public string OutArgs { get; set; }
		public string OutDescription { get; set; }
		public string OutDir { get; set; }
		public string OutIcon { get; set; }
		public string OutIconNum { get; set; }
		public long OutRunState { get; set; }
		public string OutTarget { get; set; }
	}
}