using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LBH.Data.Domain;
using LBHTenancyAPI.UseCases.Contacts.Models;

namespace LBHTenancyAPI.Gateways.Search
{
    public class SearchGateway : ISearchGateway
    {
        private readonly string _connectionString;
        public SearchGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<TenancyListItem>> SearchTenanciesAsync(SearchTenancyRequest request, CancellationToken cancellationToken)
        {
            List<TenancyListItem> results;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var all = await conn.QueryAsync<TenancyListItem>(
                    @"
                    DECLARE @Upper Integer;
                    DECLARE @Lower integer;
                    DECLARE @lowerSearchTerm nvarchar(256);

                    SET @lowerSearchTerm = LOWER(@searchTerm) 

                    if(@page = 0) 
                    begin
                    Set @Lower = 1
                    Set @Upper = @Lower + @pageSize -1
                    end
                    if(@page > 0)
                    begin
                    Set @Lower = (@pageSize * @page) + 1
                    Set @Upper = @Lower + @pageSize -1
                    end

                    SELECT
                    Seq,
                    ArrearsAgreementStartDate,
                    ArrearsAgreementStatus,
                    CurrentBalance,
                    TenancyRef,
                    PropertyRef,
                    Tenure,
                    PrimaryContactPostcode,
                    PrimaryContactShortAddress,
                    PrimaryContactName
                    FROM
                    (
                        SELECT
                        arag.arag_startdate as ArrearsAgreementStartDate,
                        arag.arag_status as ArrearsAgreementStatus,
                        tenagree.cur_bal as CurrentBalance,
                        tenagree.tag_ref as TenancyRef,
                        tenagree.prop_ref as PropertyRef,
                        tenagree.tenure as Tenure,
                        property.post_code as PrimaryContactPostcode,
                        property.short_address as PrimaryContactShortAddress,
                        RTRIM(LTRIM(member.forename)) + ' ' + RTRIM(LTRIM(member.surname)) as PrimaryContactName,
                        ROW_NUMBER() OVER (ORDER BY arag.arag_startdate DESC) AS Seq
                        FROM tenagree
                        Left JOIN dbo.member member WITH(NOLOCK)
                        ON member.house_ref = tenagree.house_ref
                        RIGHT JOIN property WITH(NOLOCK)
                        ON property.prop_ref = tenagree.prop_ref
                        RIGHt JOIN  arag AS arag WITH(NOLOCK)
                        ON arag.tag_ref = tenagree.tag_ref
                        WHERE tenagree.tag_ref = @searchTerm
                        OR member.forename = @searchTerm
                        OR member.surname = @searchTerm
                        OR property.short_address like '%'+ @searchTerm +'%'
                        OR property.post_code like  '%'+ @searchTerm +'%'
                    )
                    t
                    WHERE Seq BETWEEN @Lower AND @Upper",
                    new { searchTerm = request.SearchTerm, page = request.Page, pageSize = request.PageSize }
                ).ConfigureAwait(false);

                results = all?.ToList();
            }
            return results;
        }


    }
}