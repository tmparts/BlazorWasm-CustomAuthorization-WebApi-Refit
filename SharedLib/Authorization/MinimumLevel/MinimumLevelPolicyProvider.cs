////////////////////////////////////////////////
// � https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace SharedLib;

/// <summary>
/// ���, ������� ����� ������������ Microsoft.AspNetCore.Authorization.AuthorizationPolicy ��� ������������� �����.
/// </summary>
public class MinimumLevelPolicyProvider : IAuthorizationPolicyProvider
{
    /// <summary>
    /// ������� ����� ��������
    /// </summary>
    public static string POLICY_PREFIX => "MinimumLevel";

    /// <summary>
    /// ��������� ��������� ��������
    /// </summary>
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="options">������������� ����������� ������������, ������������ �������� Microsoft.AspNetCore.Authorization.IAuthorizationService � Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider.</param>
    public MinimumLevelPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <summary>
    /// �������� �������� �� ���������
    /// </summary>
    /// <returns>������������ ����� ���������� ����������� � ����� ��� ����, �� ������� ��� �����������, � ��� ��� ������ ���� ��������� ��� �������� �����������.</returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    /// <summary>
    /// �������� ��������� ��������
    /// </summary>
    /// <returns>������������ ����� ���������� ����������� � ����� ��� ����, �� ������� ��� �����������, � ��� ��� ������ ���� ��������� ��� �������� �����������.</returns>
    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    /// <summary>
    /// �������� ��������
    /// </summary>
    /// <param name="policyName">��� ��������</param>
    /// <returns>������������ ����� ���������� ����������� � ����� ��� ����, �� ������� ��� �����������, � ��� ��� ������ ���� ��������� ��� �������� �����������.</returns>
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            AccessLevelsUsersEnum.TryParse(policyName.Substring(POLICY_PREFIX.Length), out AccessLevelsUsersEnum level))
        {
            AuthorizationPolicyBuilder? policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new MinimumLevelRequirement(level));
            return Task.FromResult(policy.Build());
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
