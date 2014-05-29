class Scanner{
	public void work(){
	}
}

class Parser{
	public void work(){
	}
}

class ProgramNode{
	public void work(){
	}
}

class BytecodeStream{
	public void work(){
	}
}

class client{
	private Scanner scanner;
	private Parser parser;
	private ProgramNode programnode;
	private BytecodeStream bytecodestream;
	private String code;
	
	public client(Scanner s,Parser p, ProgramNode n, BytecodeStream b){
		scanner = s;
		parser = p;
		programnode = n;
		bytecodestream = b;
	}
	
	public void compile(){
	}
}