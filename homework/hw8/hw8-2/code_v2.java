public class WeatherSystem{
	WeatherData[] dataArray;
	public void checkAndDisplay(Display DP){
		int i;
		while(1){
			while(i < dataArray.length){
				if(dataArray[i].isUpdated() == true)
					DP.show();
			}
		}
	}
}

public class WeatherData{
	private float temperature;
	private float humidity;
	private float pressure;
	private int area;
	private boolean updated_flag;
	public boolean isUpdated(){
		return updated_flag;
	}
	public void track(){
		temperature = ...
		humidity = ...
		...
		updated_flag = true;
	}
	public float getTemperature(){
		return temperature;
	}
	public float get humidity(){
		return humidity;
	}
	...
	..
}

public class Display{
	private void showCurrent(){
		...
	}
	private showStatistics(){
		...
	}
	private void showForecast(){
		...
	}
	public show(WeatherData[] DataArray){
		showCurrent();
		showStatistics();
		showForecast();
	}
}