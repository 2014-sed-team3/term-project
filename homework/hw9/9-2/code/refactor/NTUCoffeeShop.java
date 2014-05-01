public class NTUCoffeeShop{
	public Product[] productOfferings;
	public NTUCoffeeShop(){
		productOfferings = null;
	}
	public float summateCost( String[] prod ){
		float sum = 0;
		for( int i=0; i<prod.length; i++ ){
			for( int j=0; j<productOfferings.length; j++ ){
				if( prod[i].equals( productOfferings[j].name ) ){
					sum += productOfferings[j].cost;
				}
			}
		}
		return sum;
	}
}