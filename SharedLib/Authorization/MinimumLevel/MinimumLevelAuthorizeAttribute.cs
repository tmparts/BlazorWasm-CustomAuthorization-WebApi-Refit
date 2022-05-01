////////////////////////////////////////////////
// � https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.AspNetCore.Authorization;

namespace SharedLib;

/// <summary>
/// ���������, ��� ��� ������ ��� ������, � �������� ����������� ���� �������, ��������� ��������� �����������.
/// </summary>
public class MinimumLevelAuthorizeAttribute : AuthorizeAttribute
{
    string POLICY_PREFIX = MinimumLevelPolicyProvider.POLICY_PREFIX;

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="level">����������� ��������� ������� �������</param>
    public MinimumLevelAuthorizeAttribute(AccessLevelsUsersEnum level) => Level = level;

    /// <summary>
    /// ������� ����������� ��������� ������� �������
    /// </summary>
    public AccessLevelsUsersEnum Level
    {
        get
        {
            if (AccessLevelsUsersEnum.TryParse(Policy.AsSpan(POLICY_PREFIX.Length), out AccessLevelsUsersEnum level))
            {
                return level;
            }
            return default;
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
