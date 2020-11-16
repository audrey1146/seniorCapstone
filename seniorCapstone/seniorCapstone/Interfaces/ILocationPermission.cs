using System;
using System.Collections.Generic;
using System.Text;

namespace seniorCapstone.Interfaces
{
	public interface ILocationPermission
	{

		public void AskForLocationPermission (Esri.ArcGISRuntime.Location.LocationDataSource myLocationDataSource);
	}
}
