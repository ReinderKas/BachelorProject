// Generated from c:\elfsquad\cpq\src\Core\Elfsquad.Core.Archer\Lang\Archer.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ArcherParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		STRING=25, NUMBER=26, BOOLEAN=27, COMMENT=28, MODIFIER=29, KEY=30, MUL=31, 
		DIV=32, MIN=33, ADD=34, GREATERTHAN=35, LESSTHAN=36, GREATERTHAN_E=37, 
		LESSTHAN_E=38, EQUALS=39, AND=40, OR=41, WS=42;
	public static final int
		RULE_prog = 0, RULE_id = 1, RULE_stat = 2, RULE_typeDeclr = 3, RULE_paramList = 4, 
		RULE_idList = 5, RULE_expr = 6, RULE_cardinExpr = 7, RULE_funcCall = 8, 
		RULE_arithExprOp = 9, RULE_equalExprOp = 10, RULE_logicalExprOp = 11, 
		RULE_propExpr = 12, RULE_obj = 13, RULE_pair = 14, RULE_array = 15, RULE_value = 16, 
		RULE_primitiveValue = 17, RULE_listCompr = 18;
	private static String[] makeRuleNames() {
		return new String[] {
			"prog", "id", "stat", "typeDeclr", "paramList", "idList", "expr", "cardinExpr", 
			"funcCall", "arithExprOp", "equalExprOp", "logicalExprOp", "propExpr", 
			"obj", "pair", "array", "value", "primitiveValue", "listCompr"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "';'", "'['", "']'", "'model'", "'{'", "'}'", "'->'", "'constraint'", 
			"'condition'", "'='", "'lower_bound'", "'upper_bound'", "'func'", "'('", 
			"','", "')'", "'=>'", "'#include'", "'int'", "'float'", "':'", "'.'", 
			"'for'", "'in'", null, null, null, null, null, null, "'*'", "'/'", "'-'", 
			"'+'", "'>'", "'<'", "'>='", "'<='", null, "'&&'", "'||'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, "STRING", "NUMBER", "BOOLEAN", "COMMENT", "MODIFIER", "KEY", "MUL", 
			"DIV", "MIN", "ADD", "GREATERTHAN", "LESSTHAN", "GREATERTHAN_E", "LESSTHAN_E", 
			"EQUALS", "AND", "OR", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "Archer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public ArcherParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class ProgContext extends ParserRuleContext {
		public List<StatContext> stat() {
			return getRuleContexts(StatContext.class);
		}
		public StatContext stat(int i) {
			return getRuleContext(StatContext.class,i);
		}
		public ProgContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_prog; }
	}

	public final ProgContext prog() throws RecognitionException {
		ProgContext _localctx = new ProgContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_prog);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(42); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(38);
				stat();
				setState(40);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__0) {
					{
					setState(39);
					match(T__0);
					}
				}

				}
				}
				setState(44); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__1) | (1L << T__3) | (1L << T__7) | (1L << T__8) | (1L << T__10) | (1L << T__11) | (1L << T__12) | (1L << T__17) | (1L << T__18) | (1L << T__19) | (1L << COMMENT))) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IdContext extends ParserRuleContext {
		public List<TerminalNode> KEY() { return getTokens(ArcherParser.KEY); }
		public TerminalNode KEY(int i) {
			return getToken(ArcherParser.KEY, i);
		}
		public IdContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_id; }
	}

	public final IdContext id() throws RecognitionException {
		IdContext _localctx = new IdContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_id);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(46);
			match(T__1);
			setState(48); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(47);
				match(KEY);
				}
				}
				setState(50); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==KEY );
			setState(52);
			match(T__2);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StatContext extends ParserRuleContext {
		public StatContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_stat; }
	 
		public StatContext() { }
		public void copyFrom(StatContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class CommentStatementContext extends StatContext {
		public TerminalNode COMMENT() { return getToken(ArcherParser.COMMENT, 0); }
		public CommentStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class ConstraintStatementContext extends StatContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public ConstraintStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class UpperBoundStatementContext extends StatContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public UpperBoundStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class ModelStatementContext extends StatContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public IdListContext idList() {
			return getRuleContext(IdListContext.class,0);
		}
		public List<StatContext> stat() {
			return getRuleContexts(StatContext.class);
		}
		public StatContext stat(int i) {
			return getRuleContext(StatContext.class,i);
		}
		public ModelStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class LowerBoundStatementContext extends StatContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public LowerBoundStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class IncludeStatementContext extends StatContext {
		public TerminalNode STRING() { return getToken(ArcherParser.STRING, 0); }
		public IncludeStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class VariableDeclarationStatementContext extends StatContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public TypeDeclrContext typeDeclr() {
			return getRuleContext(TypeDeclrContext.class,0);
		}
		public ObjContext obj() {
			return getRuleContext(ObjContext.class,0);
		}
		public VariableDeclarationStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class FuncDeclarationStatementContext extends StatContext {
		public List<TerminalNode> KEY() { return getTokens(ArcherParser.KEY); }
		public TerminalNode KEY(int i) {
			return getToken(ArcherParser.KEY, i);
		}
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public FuncDeclarationStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class RelationshipStatementContext extends StatContext {
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public TerminalNode MIN() { return getToken(ArcherParser.MIN, 0); }
		public TerminalNode KEY() { return getToken(ArcherParser.KEY, 0); }
		public CardinExprContext cardinExpr() {
			return getRuleContext(CardinExprContext.class,0);
		}
		public ObjContext obj() {
			return getRuleContext(ObjContext.class,0);
		}
		public RelationshipStatementContext(StatContext ctx) { copyFrom(ctx); }
	}
	public static class ConditionStatementContext extends StatContext {
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public ConditionStatementContext(StatContext ctx) { copyFrom(ctx); }
	}

	public final StatContext stat() throws RecognitionException {
		StatContext _localctx = new StatContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_stat);
		int _la;
		try {
			setState(130);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
			case 1:
				_localctx = new ModelStatementContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(54);
				match(T__3);
				setState(55);
				id();
				setState(57);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__20) {
					{
					setState(56);
					idList();
					}
				}

				setState(59);
				match(T__4);
				setState(64); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(60);
					stat();
					setState(62);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if (_la==T__0) {
						{
						setState(61);
						match(T__0);
						}
					}

					}
					}
					setState(66); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__1) | (1L << T__3) | (1L << T__7) | (1L << T__8) | (1L << T__10) | (1L << T__11) | (1L << T__12) | (1L << T__17) | (1L << T__18) | (1L << T__19) | (1L << COMMENT))) != 0) );
				setState(68);
				match(T__5);
				}
				break;
			case 2:
				_localctx = new RelationshipStatementContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(70);
				id();
				setState(71);
				match(MIN);
				setState(72);
				match(KEY);
				setState(74);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__13) {
					{
					setState(73);
					cardinExpr();
					}
				}

				setState(76);
				match(T__6);
				setState(77);
				id();
				setState(79);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__4) {
					{
					setState(78);
					obj();
					}
				}

				}
				break;
			case 3:
				_localctx = new VariableDeclarationStatementContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(82);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__18 || _la==T__19) {
					{
					setState(81);
					typeDeclr();
					}
				}

				setState(84);
				id();
				setState(86);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__4) {
					{
					setState(85);
					obj();
					}
				}

				}
				break;
			case 4:
				_localctx = new ConstraintStatementContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(88);
				match(T__7);
				setState(89);
				expr(0);
				}
				break;
			case 5:
				_localctx = new ConditionStatementContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(90);
				match(T__8);
				setState(91);
				id();
				setState(93);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__9) {
					{
					setState(92);
					match(T__9);
					}
				}

				setState(95);
				expr(0);
				}
				break;
			case 6:
				_localctx = new LowerBoundStatementContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(97);
				match(T__10);
				setState(98);
				id();
				setState(100);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__9) {
					{
					setState(99);
					match(T__9);
					}
				}

				setState(102);
				expr(0);
				}
				break;
			case 7:
				_localctx = new UpperBoundStatementContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(104);
				match(T__11);
				setState(105);
				id();
				setState(107);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__9) {
					{
					setState(106);
					match(T__9);
					}
				}

				setState(109);
				expr(0);
				}
				break;
			case 8:
				_localctx = new FuncDeclarationStatementContext(_localctx);
				enterOuterAlt(_localctx, 8);
				{
				setState(111);
				match(T__12);
				setState(112);
				match(KEY);
				setState(113);
				match(T__13);
				setState(122);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==KEY) {
					{
					setState(114);
					match(KEY);
					setState(119);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==T__14) {
						{
						{
						setState(115);
						match(T__14);
						setState(116);
						match(KEY);
						}
						}
						setState(121);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
				}

				setState(124);
				match(T__15);
				setState(125);
				match(T__16);
				setState(126);
				expr(0);
				}
				break;
			case 9:
				_localctx = new IncludeStatementContext(_localctx);
				enterOuterAlt(_localctx, 9);
				{
				setState(127);
				match(T__17);
				setState(128);
				match(STRING);
				}
				break;
			case 10:
				_localctx = new CommentStatementContext(_localctx);
				enterOuterAlt(_localctx, 10);
				{
				setState(129);
				match(COMMENT);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeDeclrContext extends ParserRuleContext {
		public TypeDeclrContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_typeDeclr; }
	}

	public final TypeDeclrContext typeDeclr() throws RecognitionException {
		TypeDeclrContext _localctx = new TypeDeclrContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_typeDeclr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(132);
			_la = _input.LA(1);
			if ( !(_la==T__18 || _la==T__19) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParamListContext extends ParserRuleContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public ParamListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_paramList; }
	}

	public final ParamListContext paramList() throws RecognitionException {
		ParamListContext _localctx = new ParamListContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_paramList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(134);
			expr(0);
			setState(139);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__14) {
				{
				{
				setState(135);
				match(T__14);
				setState(136);
				expr(0);
				}
				}
				setState(141);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IdListContext extends ParserRuleContext {
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public IdListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_idList; }
	}

	public final IdListContext idList() throws RecognitionException {
		IdListContext _localctx = new IdListContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_idList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(142);
			match(T__20);
			setState(143);
			id();
			setState(148);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__14) {
				{
				{
				setState(144);
				match(T__14);
				setState(145);
				id();
				}
				}
				setState(150);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExprContext extends ParserRuleContext {
		public ExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expr; }
	 
		public ExprContext() { }
		public void copyFrom(ExprContext ctx) {
			super.copyFrom(ctx);
		}
	}
	public static class FuncArgExprContext extends ExprContext {
		public TerminalNode KEY() { return getToken(ArcherParser.KEY, 0); }
		public FuncArgExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class ValueExprContext extends ExprContext {
		public PropExprContext propExpr() {
			return getRuleContext(PropExprContext.class,0);
		}
		public ValueContext value() {
			return getRuleContext(ValueContext.class,0);
		}
		public ValueExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class ListComprExprContext extends ExprContext {
		public ListComprContext listCompr() {
			return getRuleContext(ListComprContext.class,0);
		}
		public ListComprExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class FuncCallExprContext extends ExprContext {
		public FuncCallContext funcCall() {
			return getRuleContext(FuncCallContext.class,0);
		}
		public FuncCallExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class ArithBinaryExprContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public ArithExprOpContext arithExprOp() {
			return getRuleContext(ArithExprOpContext.class,0);
		}
		public ArithBinaryExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class EqualBinaryExprContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public EqualExprOpContext equalExprOp() {
			return getRuleContext(EqualExprOpContext.class,0);
		}
		public EqualBinaryExprContext(ExprContext ctx) { copyFrom(ctx); }
	}
	public static class LogicalBinaryExprContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public LogicalExprOpContext logicalExprOp() {
			return getRuleContext(LogicalExprOpContext.class,0);
		}
		public LogicalBinaryExprContext(ExprContext ctx) { copyFrom(ctx); }
	}

	public final ExprContext expr() throws RecognitionException {
		return expr(0);
	}

	private ExprContext expr(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExprContext _localctx = new ExprContext(_ctx, _parentState);
		ExprContext _prevctx = _localctx;
		int _startState = 12;
		enterRecursionRule(_localctx, 12, RULE_expr, _p);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(159);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,19,_ctx) ) {
			case 1:
				{
				_localctx = new FuncArgExprContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(152);
				match(KEY);
				}
				break;
			case 2:
				{
				_localctx = new ValueExprContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(155);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,18,_ctx) ) {
				case 1:
					{
					setState(153);
					propExpr();
					}
					break;
				case 2:
					{
					setState(154);
					value();
					}
					break;
				}
				}
				break;
			case 3:
				{
				_localctx = new FuncCallExprContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(157);
				funcCall();
				}
				break;
			case 4:
				{
				_localctx = new ListComprExprContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(158);
				listCompr();
				}
				break;
			}
			_ctx.stop = _input.LT(-1);
			setState(175);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(173);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,20,_ctx) ) {
					case 1:
						{
						_localctx = new ArithBinaryExprContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(161);
						if (!(precpred(_ctx, 7))) throw new FailedPredicateException(this, "precpred(_ctx, 7)");
						setState(162);
						arithExprOp();
						setState(163);
						expr(8);
						}
						break;
					case 2:
						{
						_localctx = new EqualBinaryExprContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(165);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(166);
						equalExprOp();
						setState(167);
						expr(6);
						}
						break;
					case 3:
						{
						_localctx = new LogicalBinaryExprContext(new ExprContext(_parentctx, _parentState));
						pushNewRecursionContext(_localctx, _startState, RULE_expr);
						setState(169);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(170);
						logicalExprOp();
						setState(171);
						expr(5);
						}
						break;
					}
					} 
				}
				setState(177);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,21,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class CardinExprContext extends ParserRuleContext {
		public List<TerminalNode> NUMBER() { return getTokens(ArcherParser.NUMBER); }
		public TerminalNode NUMBER(int i) {
			return getToken(ArcherParser.NUMBER, i);
		}
		public CardinExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_cardinExpr; }
	}

	public final CardinExprContext cardinExpr() throws RecognitionException {
		CardinExprContext _localctx = new CardinExprContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_cardinExpr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(178);
			match(T__13);
			setState(179);
			match(NUMBER);
			setState(180);
			match(T__14);
			setState(182);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NUMBER) {
				{
				setState(181);
				match(NUMBER);
				}
			}

			setState(184);
			match(T__15);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class FuncCallContext extends ParserRuleContext {
		public TerminalNode KEY() { return getToken(ArcherParser.KEY, 0); }
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public FuncCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_funcCall; }
	}

	public final FuncCallContext funcCall() throws RecognitionException {
		FuncCallContext _localctx = new FuncCallContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_funcCall);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(186);
			match(KEY);
			setState(187);
			match(T__13);
			setState(189);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__1) | (1L << T__4) | (1L << STRING) | (1L << NUMBER) | (1L << BOOLEAN) | (1L << KEY))) != 0)) {
				{
				setState(188);
				paramList();
				}
			}

			setState(191);
			match(T__15);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArithExprOpContext extends ParserRuleContext {
		public TerminalNode DIV() { return getToken(ArcherParser.DIV, 0); }
		public TerminalNode MUL() { return getToken(ArcherParser.MUL, 0); }
		public TerminalNode ADD() { return getToken(ArcherParser.ADD, 0); }
		public TerminalNode MIN() { return getToken(ArcherParser.MIN, 0); }
		public ArithExprOpContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arithExprOp; }
	}

	public final ArithExprOpContext arithExprOp() throws RecognitionException {
		ArithExprOpContext _localctx = new ArithExprOpContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_arithExprOp);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(193);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << MUL) | (1L << DIV) | (1L << MIN) | (1L << ADD))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class EqualExprOpContext extends ParserRuleContext {
		public TerminalNode GREATERTHAN() { return getToken(ArcherParser.GREATERTHAN, 0); }
		public TerminalNode LESSTHAN() { return getToken(ArcherParser.LESSTHAN, 0); }
		public TerminalNode LESSTHAN_E() { return getToken(ArcherParser.LESSTHAN_E, 0); }
		public TerminalNode GREATERTHAN_E() { return getToken(ArcherParser.GREATERTHAN_E, 0); }
		public TerminalNode EQUALS() { return getToken(ArcherParser.EQUALS, 0); }
		public EqualExprOpContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_equalExprOp; }
	}

	public final EqualExprOpContext equalExprOp() throws RecognitionException {
		EqualExprOpContext _localctx = new EqualExprOpContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_equalExprOp);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(195);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << GREATERTHAN) | (1L << LESSTHAN) | (1L << GREATERTHAN_E) | (1L << LESSTHAN_E) | (1L << EQUALS))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class LogicalExprOpContext extends ParserRuleContext {
		public TerminalNode AND() { return getToken(ArcherParser.AND, 0); }
		public TerminalNode OR() { return getToken(ArcherParser.OR, 0); }
		public LogicalExprOpContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_logicalExprOp; }
	}

	public final LogicalExprOpContext logicalExprOp() throws RecognitionException {
		LogicalExprOpContext _localctx = new LogicalExprOpContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_logicalExprOp);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(197);
			_la = _input.LA(1);
			if ( !(_la==AND || _la==OR) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PropExprContext extends ParserRuleContext {
		public List<IdContext> id() {
			return getRuleContexts(IdContext.class);
		}
		public IdContext id(int i) {
			return getRuleContext(IdContext.class,i);
		}
		public PropExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_propExpr; }
	}

	public final PropExprContext propExpr() throws RecognitionException {
		PropExprContext _localctx = new PropExprContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_propExpr);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(199);
			id();
			setState(202); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(200);
					match(T__21);
					setState(201);
					id();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(204); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,24,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ObjContext extends ParserRuleContext {
		public List<PairContext> pair() {
			return getRuleContexts(PairContext.class);
		}
		public PairContext pair(int i) {
			return getRuleContext(PairContext.class,i);
		}
		public ObjContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_obj; }
	}

	public final ObjContext obj() throws RecognitionException {
		ObjContext _localctx = new ObjContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_obj);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(206);
			match(T__4);
			setState(210);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==MODIFIER || _la==KEY) {
				{
				{
				setState(207);
				pair();
				}
				}
				setState(212);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(213);
			match(T__5);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PairContext extends ParserRuleContext {
		public TerminalNode KEY() { return getToken(ArcherParser.KEY, 0); }
		public ValueContext value() {
			return getRuleContext(ValueContext.class,0);
		}
		public TerminalNode MODIFIER() { return getToken(ArcherParser.MODIFIER, 0); }
		public PairContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_pair; }
	}

	public final PairContext pair() throws RecognitionException {
		PairContext _localctx = new PairContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_pair);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(216);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MODIFIER) {
				{
				setState(215);
				match(MODIFIER);
				}
			}

			setState(218);
			match(KEY);
			setState(219);
			match(T__9);
			setState(220);
			value();
			setState(221);
			match(T__0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArrayContext extends ParserRuleContext {
		public List<ValueContext> value() {
			return getRuleContexts(ValueContext.class);
		}
		public ValueContext value(int i) {
			return getRuleContext(ValueContext.class,i);
		}
		public ArrayContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_array; }
	}

	public final ArrayContext array() throws RecognitionException {
		ArrayContext _localctx = new ArrayContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_array);
		int _la;
		try {
			setState(236);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,28,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(223);
				match(T__1);
				setState(224);
				value();
				setState(229);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__14) {
					{
					{
					setState(225);
					match(T__14);
					setState(226);
					value();
					}
					}
					setState(231);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(232);
				match(T__2);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(234);
				match(T__1);
				setState(235);
				match(T__2);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ValueContext extends ParserRuleContext {
		public PrimitiveValueContext primitiveValue() {
			return getRuleContext(PrimitiveValueContext.class,0);
		}
		public ObjContext obj() {
			return getRuleContext(ObjContext.class,0);
		}
		public ArrayContext array() {
			return getRuleContext(ArrayContext.class,0);
		}
		public ValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value; }
	}

	public final ValueContext value() throws RecognitionException {
		ValueContext _localctx = new ValueContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_value);
		try {
			setState(241);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case STRING:
			case NUMBER:
			case BOOLEAN:
				enterOuterAlt(_localctx, 1);
				{
				setState(238);
				primitiveValue();
				}
				break;
			case T__4:
				enterOuterAlt(_localctx, 2);
				{
				setState(239);
				obj();
				}
				break;
			case T__1:
				enterOuterAlt(_localctx, 3);
				{
				setState(240);
				array();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class PrimitiveValueContext extends ParserRuleContext {
		public TerminalNode STRING() { return getToken(ArcherParser.STRING, 0); }
		public TerminalNode NUMBER() { return getToken(ArcherParser.NUMBER, 0); }
		public TerminalNode BOOLEAN() { return getToken(ArcherParser.BOOLEAN, 0); }
		public PrimitiveValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_primitiveValue; }
	}

	public final PrimitiveValueContext primitiveValue() throws RecognitionException {
		PrimitiveValueContext _localctx = new PrimitiveValueContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_primitiveValue);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(243);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << STRING) | (1L << NUMBER) | (1L << BOOLEAN))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ListComprContext extends ParserRuleContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public IdContext id() {
			return getRuleContext(IdContext.class,0);
		}
		public ListComprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_listCompr; }
	}

	public final ListComprContext listCompr() throws RecognitionException {
		ListComprContext _localctx = new ListComprContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_listCompr);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(245);
			match(T__4);
			setState(246);
			expr(0);
			setState(247);
			match(T__22);
			setState(248);
			id();
			setState(249);
			match(T__23);
			setState(250);
			expr(0);
			setState(251);
			match(T__5);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 6:
			return expr_sempred((ExprContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expr_sempred(ExprContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 7);
		case 1:
			return precpred(_ctx, 5);
		case 2:
			return precpred(_ctx, 4);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3,\u0100\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\3\2\3\2\5\2+\n\2\6\2-\n\2\r\2\16\2.\3\3\3\3\6\3\63"+
		"\n\3\r\3\16\3\64\3\3\3\3\3\4\3\4\3\4\5\4<\n\4\3\4\3\4\3\4\5\4A\n\4\6\4"+
		"C\n\4\r\4\16\4D\3\4\3\4\3\4\3\4\3\4\3\4\5\4M\n\4\3\4\3\4\3\4\5\4R\n\4"+
		"\3\4\5\4U\n\4\3\4\3\4\5\4Y\n\4\3\4\3\4\3\4\3\4\3\4\5\4`\n\4\3\4\3\4\3"+
		"\4\3\4\3\4\5\4g\n\4\3\4\3\4\3\4\3\4\3\4\5\4n\n\4\3\4\3\4\3\4\3\4\3\4\3"+
		"\4\3\4\3\4\7\4x\n\4\f\4\16\4{\13\4\5\4}\n\4\3\4\3\4\3\4\3\4\3\4\3\4\5"+
		"\4\u0085\n\4\3\5\3\5\3\6\3\6\3\6\7\6\u008c\n\6\f\6\16\6\u008f\13\6\3\7"+
		"\3\7\3\7\3\7\7\7\u0095\n\7\f\7\16\7\u0098\13\7\3\b\3\b\3\b\3\b\5\b\u009e"+
		"\n\b\3\b\3\b\5\b\u00a2\n\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b\3\b"+
		"\3\b\7\b\u00b0\n\b\f\b\16\b\u00b3\13\b\3\t\3\t\3\t\3\t\5\t\u00b9\n\t\3"+
		"\t\3\t\3\n\3\n\3\n\5\n\u00c0\n\n\3\n\3\n\3\13\3\13\3\f\3\f\3\r\3\r\3\16"+
		"\3\16\3\16\6\16\u00cd\n\16\r\16\16\16\u00ce\3\17\3\17\7\17\u00d3\n\17"+
		"\f\17\16\17\u00d6\13\17\3\17\3\17\3\20\5\20\u00db\n\20\3\20\3\20\3\20"+
		"\3\20\3\20\3\21\3\21\3\21\3\21\7\21\u00e6\n\21\f\21\16\21\u00e9\13\21"+
		"\3\21\3\21\3\21\3\21\5\21\u00ef\n\21\3\22\3\22\3\22\5\22\u00f4\n\22\3"+
		"\23\3\23\3\24\3\24\3\24\3\24\3\24\3\24\3\24\3\24\3\24\2\3\16\25\2\4\6"+
		"\b\n\f\16\20\22\24\26\30\32\34\36 \"$&\2\7\3\2\25\26\3\2!$\3\2%)\3\2*"+
		"+\3\2\33\35\2\u0116\2,\3\2\2\2\4\60\3\2\2\2\6\u0084\3\2\2\2\b\u0086\3"+
		"\2\2\2\n\u0088\3\2\2\2\f\u0090\3\2\2\2\16\u00a1\3\2\2\2\20\u00b4\3\2\2"+
		"\2\22\u00bc\3\2\2\2\24\u00c3\3\2\2\2\26\u00c5\3\2\2\2\30\u00c7\3\2\2\2"+
		"\32\u00c9\3\2\2\2\34\u00d0\3\2\2\2\36\u00da\3\2\2\2 \u00ee\3\2\2\2\"\u00f3"+
		"\3\2\2\2$\u00f5\3\2\2\2&\u00f7\3\2\2\2(*\5\6\4\2)+\7\3\2\2*)\3\2\2\2*"+
		"+\3\2\2\2+-\3\2\2\2,(\3\2\2\2-.\3\2\2\2.,\3\2\2\2./\3\2\2\2/\3\3\2\2\2"+
		"\60\62\7\4\2\2\61\63\7 \2\2\62\61\3\2\2\2\63\64\3\2\2\2\64\62\3\2\2\2"+
		"\64\65\3\2\2\2\65\66\3\2\2\2\66\67\7\5\2\2\67\5\3\2\2\289\7\6\2\29;\5"+
		"\4\3\2:<\5\f\7\2;:\3\2\2\2;<\3\2\2\2<=\3\2\2\2=B\7\7\2\2>@\5\6\4\2?A\7"+
		"\3\2\2@?\3\2\2\2@A\3\2\2\2AC\3\2\2\2B>\3\2\2\2CD\3\2\2\2DB\3\2\2\2DE\3"+
		"\2\2\2EF\3\2\2\2FG\7\b\2\2G\u0085\3\2\2\2HI\5\4\3\2IJ\7#\2\2JL\7 \2\2"+
		"KM\5\20\t\2LK\3\2\2\2LM\3\2\2\2MN\3\2\2\2NO\7\t\2\2OQ\5\4\3\2PR\5\34\17"+
		"\2QP\3\2\2\2QR\3\2\2\2R\u0085\3\2\2\2SU\5\b\5\2TS\3\2\2\2TU\3\2\2\2UV"+
		"\3\2\2\2VX\5\4\3\2WY\5\34\17\2XW\3\2\2\2XY\3\2\2\2Y\u0085\3\2\2\2Z[\7"+
		"\n\2\2[\u0085\5\16\b\2\\]\7\13\2\2]_\5\4\3\2^`\7\f\2\2_^\3\2\2\2_`\3\2"+
		"\2\2`a\3\2\2\2ab\5\16\b\2b\u0085\3\2\2\2cd\7\r\2\2df\5\4\3\2eg\7\f\2\2"+
		"fe\3\2\2\2fg\3\2\2\2gh\3\2\2\2hi\5\16\b\2i\u0085\3\2\2\2jk\7\16\2\2km"+
		"\5\4\3\2ln\7\f\2\2ml\3\2\2\2mn\3\2\2\2no\3\2\2\2op\5\16\b\2p\u0085\3\2"+
		"\2\2qr\7\17\2\2rs\7 \2\2s|\7\20\2\2ty\7 \2\2uv\7\21\2\2vx\7 \2\2wu\3\2"+
		"\2\2x{\3\2\2\2yw\3\2\2\2yz\3\2\2\2z}\3\2\2\2{y\3\2\2\2|t\3\2\2\2|}\3\2"+
		"\2\2}~\3\2\2\2~\177\7\22\2\2\177\u0080\7\23\2\2\u0080\u0085\5\16\b\2\u0081"+
		"\u0082\7\24\2\2\u0082\u0085\7\33\2\2\u0083\u0085\7\36\2\2\u00848\3\2\2"+
		"\2\u0084H\3\2\2\2\u0084T\3\2\2\2\u0084Z\3\2\2\2\u0084\\\3\2\2\2\u0084"+
		"c\3\2\2\2\u0084j\3\2\2\2\u0084q\3\2\2\2\u0084\u0081\3\2\2\2\u0084\u0083"+
		"\3\2\2\2\u0085\7\3\2\2\2\u0086\u0087\t\2\2\2\u0087\t\3\2\2\2\u0088\u008d"+
		"\5\16\b\2\u0089\u008a\7\21\2\2\u008a\u008c\5\16\b\2\u008b\u0089\3\2\2"+
		"\2\u008c\u008f\3\2\2\2\u008d\u008b\3\2\2\2\u008d\u008e\3\2\2\2\u008e\13"+
		"\3\2\2\2\u008f\u008d\3\2\2\2\u0090\u0091\7\27\2\2\u0091\u0096\5\4\3\2"+
		"\u0092\u0093\7\21\2\2\u0093\u0095\5\4\3\2\u0094\u0092\3\2\2\2\u0095\u0098"+
		"\3\2\2\2\u0096\u0094\3\2\2\2\u0096\u0097\3\2\2\2\u0097\r\3\2\2\2\u0098"+
		"\u0096\3\2\2\2\u0099\u009a\b\b\1\2\u009a\u00a2\7 \2\2\u009b\u009e\5\32"+
		"\16\2\u009c\u009e\5\"\22\2\u009d\u009b\3\2\2\2\u009d\u009c\3\2\2\2\u009e"+
		"\u00a2\3\2\2\2\u009f\u00a2\5\22\n\2\u00a0\u00a2\5&\24\2\u00a1\u0099\3"+
		"\2\2\2\u00a1\u009d\3\2\2\2\u00a1\u009f\3\2\2\2\u00a1\u00a0\3\2\2\2\u00a2"+
		"\u00b1\3\2\2\2\u00a3\u00a4\f\t\2\2\u00a4\u00a5\5\24\13\2\u00a5\u00a6\5"+
		"\16\b\n\u00a6\u00b0\3\2\2\2\u00a7\u00a8\f\7\2\2\u00a8\u00a9\5\26\f\2\u00a9"+
		"\u00aa\5\16\b\b\u00aa\u00b0\3\2\2\2\u00ab\u00ac\f\6\2\2\u00ac\u00ad\5"+
		"\30\r\2\u00ad\u00ae\5\16\b\7\u00ae\u00b0\3\2\2\2\u00af\u00a3\3\2\2\2\u00af"+
		"\u00a7\3\2\2\2\u00af\u00ab\3\2\2\2\u00b0\u00b3\3\2\2\2\u00b1\u00af\3\2"+
		"\2\2\u00b1\u00b2\3\2\2\2\u00b2\17\3\2\2\2\u00b3\u00b1\3\2\2\2\u00b4\u00b5"+
		"\7\20\2\2\u00b5\u00b6\7\34\2\2\u00b6\u00b8\7\21\2\2\u00b7\u00b9\7\34\2"+
		"\2\u00b8\u00b7\3\2\2\2\u00b8\u00b9\3\2\2\2\u00b9\u00ba\3\2\2\2\u00ba\u00bb"+
		"\7\22\2\2\u00bb\21\3\2\2\2\u00bc\u00bd\7 \2\2\u00bd\u00bf\7\20\2\2\u00be"+
		"\u00c0\5\n\6\2\u00bf\u00be\3\2\2\2\u00bf\u00c0\3\2\2\2\u00c0\u00c1\3\2"+
		"\2\2\u00c1\u00c2\7\22\2\2\u00c2\23\3\2\2\2\u00c3\u00c4\t\3\2\2\u00c4\25"+
		"\3\2\2\2\u00c5\u00c6\t\4\2\2\u00c6\27\3\2\2\2\u00c7\u00c8\t\5\2\2\u00c8"+
		"\31\3\2\2\2\u00c9\u00cc\5\4\3\2\u00ca\u00cb\7\30\2\2\u00cb\u00cd\5\4\3"+
		"\2\u00cc\u00ca\3\2\2\2\u00cd\u00ce\3\2\2\2\u00ce\u00cc\3\2\2\2\u00ce\u00cf"+
		"\3\2\2\2\u00cf\33\3\2\2\2\u00d0\u00d4\7\7\2\2\u00d1\u00d3\5\36\20\2\u00d2"+
		"\u00d1\3\2\2\2\u00d3\u00d6\3\2\2\2\u00d4\u00d2\3\2\2\2\u00d4\u00d5\3\2"+
		"\2\2\u00d5\u00d7\3\2\2\2\u00d6\u00d4\3\2\2\2\u00d7\u00d8\7\b\2\2\u00d8"+
		"\35\3\2\2\2\u00d9\u00db\7\37\2\2\u00da\u00d9\3\2\2\2\u00da\u00db\3\2\2"+
		"\2\u00db\u00dc\3\2\2\2\u00dc\u00dd\7 \2\2\u00dd\u00de\7\f\2\2\u00de\u00df"+
		"\5\"\22\2\u00df\u00e0\7\3\2\2\u00e0\37\3\2\2\2\u00e1\u00e2\7\4\2\2\u00e2"+
		"\u00e7\5\"\22\2\u00e3\u00e4\7\21\2\2\u00e4\u00e6\5\"\22\2\u00e5\u00e3"+
		"\3\2\2\2\u00e6\u00e9\3\2\2\2\u00e7\u00e5\3\2\2\2\u00e7\u00e8\3\2\2\2\u00e8"+
		"\u00ea\3\2\2\2\u00e9\u00e7\3\2\2\2\u00ea\u00eb\7\5\2\2\u00eb\u00ef\3\2"+
		"\2\2\u00ec\u00ed\7\4\2\2\u00ed\u00ef\7\5\2\2\u00ee\u00e1\3\2\2\2\u00ee"+
		"\u00ec\3\2\2\2\u00ef!\3\2\2\2\u00f0\u00f4\5$\23\2\u00f1\u00f4\5\34\17"+
		"\2\u00f2\u00f4\5 \21\2\u00f3\u00f0\3\2\2\2\u00f3\u00f1\3\2\2\2\u00f3\u00f2"+
		"\3\2\2\2\u00f4#\3\2\2\2\u00f5\u00f6\t\6\2\2\u00f6%\3\2\2\2\u00f7\u00f8"+
		"\7\7\2\2\u00f8\u00f9\5\16\b\2\u00f9\u00fa\7\31\2\2\u00fa\u00fb\5\4\3\2"+
		"\u00fb\u00fc\7\32\2\2\u00fc\u00fd\5\16\b\2\u00fd\u00fe\7\b\2\2\u00fe\'"+
		"\3\2\2\2 *.\64;@DLQTX_fmy|\u0084\u008d\u0096\u009d\u00a1\u00af\u00b1\u00b8"+
		"\u00bf\u00ce\u00d4\u00da\u00e7\u00ee\u00f3";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}