public class Composition{
    private Line[] lines = new Line[100];
	private Component[] components = new Component[100];
    
    public Composition(int num_line, int num_component){
        for(int i=0;i<num_line;i++){
            lines[i] = new Line(3);
        }
        for(int i=0;i<num_component;i++){
            components[i] = new Component();
        }
    }
	public void arrange(Strategy s){
        lines = s.arrange(components);
	}
}
