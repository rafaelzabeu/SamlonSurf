public class Token  {

    public TokenType tokenType
    {
        get;
        private set;
    }
    public OperatorType operatorType
    {
        get;
        private set;
    }
    public int precedence
    {
        get;
        private set;
    }
    public float value
    {
        get;
        private set;
    }
    public string token
    {
        get;
        private set;
    }
    public bool leftAssociative
    {
        get;
        private set;
    }

    public Token(string token)
    {
        initialize(token.ToLower());
    }

    public Token(char token)
    {
        initialize(token.ToString());
    }
	
    private void initialize(string token)
    {
        this.token = token;
        setTypes(token);
        precedence = findPrecedence(operatorType);
        leftAssociative = !(operatorType == OperatorType.Power || operatorType == OperatorType.SquareRoot);
    }

    private void setTypes(string token)
    {
        float tryValue;
        if(token == "x")
        {
            tokenType = TokenType.Variable;
            operatorType = OperatorType.None;
        }
        else if(float.TryParse(token,out tryValue))
        {
            value = tryValue;
            tokenType = TokenType.Number;
            operatorType = OperatorType.None;
        }
        else if(token == "(" || token == ")")
        {
            tokenType = TokenType.Parentesis;
            operatorType = OperatorType.None;
        }
        else
        {
            tokenType = TokenType.Operator;
            operatorType = findOperatorType(token);
        }
    }

    private OperatorType findOperatorType(string token)
    {
        switch(token)
        {
            case "+":
                return OperatorType.Add;
            case "-":
                return OperatorType.Subtract;
            case "*":
                return OperatorType.Multiply;
            case "/":
                return OperatorType.Divide;
            case "^":
                return OperatorType.Power;
            //case "sqr":
            //    return OperatorType.SquareRoot;
            default:
                return OperatorType.None;
        }
    }

    private int findPrecedence(OperatorType type)
    {
        switch (type)
        {
            case OperatorType.None:
                return -1;
            case OperatorType.Add:
                return 0;
            case OperatorType.Subtract:
                return 0;
            case OperatorType.Multiply:
                return 1;
            case OperatorType.Divide:
                return 1;
            case OperatorType.Power:
                return 2;
            case OperatorType.SquareRoot:
                return 2;
            default:
                return -1;
        }
    }
}
