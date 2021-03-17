using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_Templates2_TemplateId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_Templates2_TemplateId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates2_Tenants_CreaterTenantId",
                table: "Templates2");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates2_Users_CreaterUserId",
                table: "Templates2");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateVersions_Templates2_TemplateId",
                table: "TemplateVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Templates2",
                table: "Templates2");

            migrationBuilder.RenameTable(
                name: "Templates2",
                newName: "Templates");

            migrationBuilder.RenameIndex(
                name: "IX_Templates2_CreaterUserId",
                table: "Templates",
                newName: "IX_Templates_CreaterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Templates2_CreaterTenantId",
                table: "Templates",
                newName: "IX_Templates_CreaterTenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Templates",
                table: "Templates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_Templates_TemplateId",
                table: "Experiment",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_Templates_TemplateId",
                table: "ExperimentPreprocess",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Tenants_CreaterTenantId",
                table: "Templates",
                column: "CreaterTenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Users_CreaterUserId",
                table: "Templates",
                column: "CreaterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateVersions_Templates_TemplateId",
                table: "TemplateVersions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiment_Templates_TemplateId",
                table: "Experiment");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocess_Templates_TemplateId",
                table: "ExperimentPreprocess");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Tenants_CreaterTenantId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Users_CreaterUserId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateVersions_Templates_TemplateId",
                table: "TemplateVersions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Templates",
                table: "Templates");

            migrationBuilder.RenameTable(
                name: "Templates",
                newName: "Templates2");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_CreaterUserId",
                table: "Templates2",
                newName: "IX_Templates2_CreaterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_CreaterTenantId",
                table: "Templates2",
                newName: "IX_Templates2_CreaterTenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Templates2",
                table: "Templates2",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiment_Templates2_TemplateId",
                table: "Experiment",
                column: "TemplateId",
                principalTable: "Templates2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocess_Templates2_TemplateId",
                table: "ExperimentPreprocess",
                column: "TemplateId",
                principalTable: "Templates2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates2_Tenants_CreaterTenantId",
                table: "Templates2",
                column: "CreaterTenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates2_Users_CreaterUserId",
                table: "Templates2",
                column: "CreaterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateVersions_Templates2_TemplateId",
                table: "TemplateVersions",
                column: "TemplateId",
                principalTable: "Templates2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
