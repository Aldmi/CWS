
1.
��� ByRulesExchangeSpBehavior.
� ������ Exchange ���������� ������ ������ (ExchangeRules), � ������ ������� ������ (������, �����, ����� �� �����, ������).
����� �������� Resolution. ������� �����������, ���� �������� ������� � Resolution.

Resolution �������� � ������������ ����� (�� ������� ����� ��-� �� uit)

Resolution= "{I}>20 || {U<100}"  //������� ���������� ���� ��� > 20 ��� ����. < 100

//������� ������� ��� ���������
var selectedRules = ExchangeRules.Where(rule => rule.CheckResolution<MyType>(data)).ToList();

CheckResolution(T data)
{
  ����� ��������� ��� ��� ���-�� � ������ I � U.
  ���� ��� �������� � ������� � ������
  ���������� ��������  I � U � ��������  "{I}>20 || {U<100}".
  ������� ��������� ��� �����.
}

2.
IExhangeBehavior
����� ��������� ������ ������� ��-��.
��� SerialPort:
������� ������ ������ ���� (�.�. ���� rules, ��� ����� �������� �������� ������/�����).
rule - ���� Command = "reset"                     - ����������� ����� �������� �������
rule - ���� Resolution = "������ ���������"       - ����������� ����� ��������� == true
rulr - ------                                     - ����������� ������.



�������� DockerImage �������� build:
D:\Git\CWS\src> docker build -t webapiswc -f Dockerfile .