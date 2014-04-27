public class Beverage{
	String name;
	int cost;
	public Beverage(String n,int c){
		name = n;
		cost = c;
	}
}

public class Condiment{
	String name;
	int cost;
	public Condiment(String n,int c){
		name = n;
		cost = c;
	}
}

public class Order{
	Beverage[] b;
	Condiment[] c;
	
	public int summate(){
		int sum = 0;
		for(int i=0;i < b.length;i++)
			sum += b[i].cost;
		for(int i=0;i < c.length;i++)
			sum += c[i].cost;
		return sum;
	}
}