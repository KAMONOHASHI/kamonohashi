using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetId",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_DataSets_InputDataSetId",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_Templates_TemplateId",
                table: "ExperimentHistories");

            migrationBuilder.AlterColumn<long>(
                name: "TemplateId",
                table: "ExperimentHistories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InputDataSetId",
                table: "ExperimentHistories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DataSetVersionId",
                table: "ExperimentHistories",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_DataSetVersionId",
                table: "ExperimentHistories",
                column: "DataSetVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasets_DataSetId",
                table: "ExperimentHistories",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetVersionId",
                table: "ExperimentHistories",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_DataSets_InputDataSetId",
                table: "ExperimentHistories",
                column: "InputDataSetId",
                principalTable: "DataSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_Templates_TemplateId",
                table: "ExperimentHistories",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasets_DataSetId",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetVersionId",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_DataSets_InputDataSetId",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_Templates_TemplateId",
                table: "ExperimentHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExperimentHistories_DataSetVersionId",
                table: "ExperimentHistories");

            migrationBuilder.DropColumn(
                name: "DataSetVersionId",
                table: "ExperimentHistories");

            migrationBuilder.AlterColumn<long>(
                name: "TemplateId",
                table: "ExperimentHistories",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "InputDataSetId",
                table: "ExperimentHistories",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetId",
                table: "ExperimentHistories",
                column: "DataSetId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_DataSets_InputDataSetId",
                table: "ExperimentHistories",
                column: "InputDataSetId",
                principalTable: "DataSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_Templates_TemplateId",
                table: "ExperimentHistories",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
