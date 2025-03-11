namespace LogParser.Test;

public class CouplingTest
{
    private const string ExampleData = """
                                       --d2b76e90--2023-02-08--chiarapascucci-gds
                                       16	10	src/components/check-your-phone/check-your-phone-controller.ts
                                       10	16	src/components/check-your-phone/check-your-phone-service.ts
                                       11	3	src/components/check-your-phone/tests/check-your-phone-service.test.ts
                                       4	8	src/components/check-your-phone/types.ts
                                       14	0	src/utils/types.ts
                                       
                                       --6c95dd7d--2022-09-06--dbes-gds
                                       --9df61c97--2022-09-06--dbes-gds
                                       3	1	src/components/change-email/change-email-controller.ts
                                       4	2	src/components/change-email/change-email-service.ts
                                       2	1	src/components/change-email/tests/change-email-controller.test.ts
                                       2	1	src/components/change-email/types.ts
                                       3	1	src/components/change-password/change-password-controller.ts
                                       4	2	src/components/change-password/change-password-service.ts
                                       5	1	src/components/change-password/tests/change-password-controller.test.ts
                                       2	1	src/components/change-password/types.ts
                                       3	1	src/components/change-phone-number/change-phone-number-controller.ts
                                       4	2	src/components/change-phone-number/change-phone-number-service.ts
                                       1	0	src/components/change-phone-number/tests/change-phone-number-controller.test.ts
                                       2	1	src/components/change-phone-number/types.ts
                                       3	1	src/components/check-your-email/check-your-email-controller.ts
                                       4	2	src/components/check-your-email/check-your-email-service.ts
                                       1	0	src/components/check-your-email/tests/check-your-email-controller.test.ts
                                       2	1	src/components/check-your-email/types.ts
                                       16	14	src/components/check-your-phone/check-your-phone-controller.ts
                                       32	30	src/components/check-your-phone/check-your-phone-service.ts
                                       2	1	src/components/check-your-phone/tests/check-your-phone-controller.test.ts
                                       114	114	src/components/check-your-phone/tests/check-your-phone-integration.test.ts
                                       8	7	src/components/check-your-phone/types.ts
                                       1	1	src/components/delete-account/delete-account-controller.ts
                                       1	1	src/components/manage-your-account/tests/manage-your-account-controller.test.ts
                                       6	1	src/utils/http.ts
                                       
                                       --9764df5e--2024-05-07--peterfajemisincabinetoffice
                                       --9b7d4162--2024-04-26--Peter Fajemisin
                                       1	0	src/app.constants.ts
                                       22	4	src/components/check-your-phone/check-your-phone-controller.ts
                                       2	0	src/components/check-your-phone/check-your-phone-routes.ts
                                       9	0	src/components/check-your-phone/check-your-phone-service.ts
                                       36	1	src/components/check-your-phone/tests/check-your-phone-controller.test.ts
                                       39	38	src/components/check-your-phone/tests/check-your-phone-integration.test.ts
                                       58	1	src/components/check-your-phone/tests/check-your-phone-service.test.ts
                                       5	0	src/components/check-your-phone/types.ts
                                       1	17	src/components/security/security-routes.ts
                                       20	3	src/middleware/mfa-method-middleware.ts
                                       71	14	src/utils/mfa/index.ts
                                       178	9	src/utils/test/mfa-test.ts
                                       2	0	src/utils/types.ts
                                       
                                       --cf179aa4--2024-06-25--Saral Kaushik
                                       --d2100399--2024-06-25--Alfonso Enciso
                                       --a521ddac--2024-06-20--Alfonso Patino
                                       1	1	deploy/template.yaml
                                       1	1	docker-compose.yml
                                       1	1	local.Dockerfile
                                       20	12	src/components/add-mfa-method-app/add-mfa-method-app-controller.ts
                                       28	10	src/components/add-mfa-method-app/tests/add-mfa-methods-app-controller.test.ts
                                       68	16	src/components/add-mfa-method-sms/add-mfa-method-sms-controller.ts
                                       2	1	src/components/add-mfa-method-sms/add-mfa-method-sms-routes.ts
                                       8	2	src/components/add-mfa-method-sms/tests/add-mfa-method-sms-controller.test.ts
                                       20	2	src/components/check-your-phone/check-your-phone-controller.ts
                                       9	1	src/components/check-your-phone/check-your-phone-service.ts
                                       3	0	src/components/check-your-phone/tests/check-your-phone-controller.test.ts
                                       5	0	src/components/check-your-phone/types.ts
                                       2	2	src/components/common/layout/base.njk
                                       1	1	src/locales/en/translation.json
                                       32	38	src/utils/mfa/index.ts
                                       3	2	src/utils/mfa/types.d.ts
                                       1	0	src/utils/types.ts
                                       """;
    
    private readonly List<Block> _blocks = BlockParser.GetBlocks(ExampleData);

    [Fact]
    public void Has8Entries()
    {
        var couplingAnalysisResults = CouplingAnalysis.Analyse(_blocks);
        
    }
}

public class CouplingAnalysis
{
    public static CouplingAnalysis Analyse(List<Block> blocks)
    {
        throw new NotImplementedException();
    }
}