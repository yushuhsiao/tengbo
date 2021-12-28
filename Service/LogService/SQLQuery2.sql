select max(len(UserName)) UserName, max(len(GameType)) GameType, max(len(Result)) Result, max(len(SerialID)) SerialID, max(len(RoundNo)) RoundNo,max(len(ResultType)) ResultType, max(len([Card])) [Card], max(len(IsPaid)) IsPaid, max(len(GameCode)) GameCode from (
--select * from (
SELECT  1 gametype,gamekind,UserName,WagersID,WagersDate,GameType,Result,BetAmount,Payoff,Currency,null ExchangeRate,CreateTime,     Commissionable,null Commission,null SerialID,null RoundNo,null GameCode,null ResultType,null [Card],null IsPaid FROM gamelog.dbo.bbin_BetRecord01 nolock union
SELECT  2 gametype,gamekind,UserName,WagersID,WagersDate,GameType,Result,BetAmount,Payoff,Currency,     ExchangeRate,CreateTime,null Commissionable,null Commission,null SerialID,null RoundNo,null GameCode,null ResultType,null [Card],null IsPaid FROM gamelog.dbo.bbin_BetRecord02 nolock union
SELECT  3 gametype,gamekind,UserName,WagersID,WagersDate,GameType,Result,BetAmount,Payoff,Currency,     ExchangeRate,CreateTime,     Commissionable,null Commission,     SerialID,     RoundNo,     GameCode,     ResultType,     [Card],null IsPaid FROM gamelog.dbo.bbin_BetRecord03 nolock union
SELECT  5 gametype,gamekind,UserName,WagersID,WagersDate,GameType,Result,BetAmount,Payoff,Currency,     ExchangeRate,CreateTime,     Commissionable,null Commission,null SerialID,null RoundNo,null GameCode,null ResultType,null [Card],null IsPaid FROM gamelog.dbo.bbin_BetRecord05 nolock union
SELECT 12 gametype,gamekind,UserName,WagersID,WagersDate,GameType,Result,BetAmount,Payoff,Currency,     ExchangeRate,CreateTime,null Commissionable,     Commission,null SerialID,null RoundNo,null GameCode,null ResultType,null [Card],     IsPaid FROM gamelog.dbo.bbin_BetRecord12 nolock
) a
--order by CreateTime desc

--select * from(
--select gamekind,UserName, dateadd(dd,datediff(dd,0,WagersDate),0) ACTime, sum(BetAmount) BetAmount,sum(Commissionable) BetAmountAct, sum(Payoff) Payoff from bbin_BetRecord01 nolock group by gamekind, UserName, dateadd(dd,datediff(dd,0,WagersDate),0) union
--select gamekind,UserName, dateadd(dd,datediff(dd,0,WagersDate),0) ACTime, sum(BetAmount) BetAmount,0 BetAmountAct, sum(Payoff) Payoff from bbin_BetRecord02 nolock group by gamekind, UserName, dateadd(dd,datediff(dd,0,WagersDate),0) union
--select gamekind,UserName, dateadd(dd,datediff(dd,0,WagersDate),0) ACTime, sum(BetAmount) BetAmount,sum(Commissionable) BetAmountAct, sum(Payoff) Payoff from bbin_BetRecord03 nolock group by gamekind, UserName, dateadd(dd,datediff(dd,0,WagersDate),0) union
--select gamekind,UserName, dateadd(dd,datediff(dd,0,WagersDate),0) ACTime, sum(BetAmount) BetAmount,sum(Commissionable) BetAmountAct, sum(Payoff) Payoff from bbin_BetRecord05 nolock group by gamekind, UserName, dateadd(dd,datediff(dd,0,WagersDate),0) union
--select gamekind,UserName, dateadd(dd,datediff(dd,0,WagersDate),0) ACTime, 0 BetAmount,0 BetAmountAct, sum(Payoff) Payoff from bbin_BetRecord12 nolock group by gamekind, UserName, dateadd(dd,datediff(dd,0,WagersDate),0)
--) a
--order by ACTime desc