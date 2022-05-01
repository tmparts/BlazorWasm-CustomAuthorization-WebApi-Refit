////////////////////////////////////////////////
// � https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.AspNetCore.Authorization;

namespace SharedLib;

/// <summary>
/// ������������ ���������� �����������.
/// </summary>
public class MinimumLevelRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// ����������� ��������� ������� ����
    /// </summary>
    public AccessLevelsUsersEnum Level { get; private set; }

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="level">����������� ��������� ������� ����</param>
    public MinimumLevelRequirement(AccessLevelsUsersEnum level) { Level = level; }
}
