using System.Collections.Generic;

namespace Tobii.Gaming.Internal
{
	internal class DataProviderStub<T> : IDataProvider<T> where T : ITimestamped
	{
		// --------------------------------------------------------------------
		//  Implementation of IDataProvider<T>
		// --------------------------------------------------------------------

		public T Last { get; protected set; }

		public IEnumerable<T> GetDataPointsSince(ITimestamped dataPoint)
		{
			return new List<T>();
		}
	}

	internal class GazePointDataProviderStub : DataProviderStub<GazePoint>
	{
		public GazePointDataProviderStub()
		{
			Last = GazePoint.Invalid;
		}
	}

	internal class HeadPoseDataProviderStub : DataProviderStub<HeadPose>
	{
		public HeadPoseDataProviderStub()
		{
			Last = HeadPose.Invalid;
		}
	}
}
