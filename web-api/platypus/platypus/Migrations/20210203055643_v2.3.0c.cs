using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.RenameColumn(
                name: "ExperimentHistoryId",
                table: "ExperimentPreprocessHistoryOutput",
                newName: "ExperimentPreprocessedHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_ExperimentHistoryId",
                table: "ExperimentPreprocessHistoryOutput",
                newName: "IX_ExperimentPreprocessHistoryOutput_ExperimentPreprocessedHis~");

            migrationBuilder.AddColumn<long>(
                name: "DataSetVersionId",
                table: "ExperimentPreprocessHistories",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OutputDataSetId",
                table: "ExperimentPreprocessHistories",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExperimentPreprocessHistoryId",
                table: "ExperimentHistories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_DataSetVersionId",
                table: "ExperimentPreprocessHistories",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_OutputDataSetId",
                table: "ExperimentPreprocessHistories",
                column: "OutputDataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_ExperimentPreprocessHistoryId",
                table: "ExperimentHistories",
                column: "ExperimentPreprocessHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentHistories_ExperimentPreprocessHistories_Experimen~",
                table: "ExperimentHistories",
                column: "ExperimentPreprocessHistoryId",
                principalTable: "ExperimentPreprocessHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocessHistories",
                column: "DataSetId",
                principalTable: "AquariumDatasets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                table: "ExperimentPreprocessHistories",
                column: "DataSetVersionId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocessHistories_DataSets_OutputDataSetId",
                table: "ExperimentPreprocessHistories",
                column: "OutputDataSetId",
                principalTable: "DataSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentHistories_ExperimentPreprocessHistories_Experimen~",
                table: "ExperimentHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasets_DataSetId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentPreprocessHistories_DataSets_OutputDataSetId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExperimentPreprocessHistories_DataSetVersionId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExperimentPreprocessHistories_OutputDataSetId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropIndex(
                name: "IX_ExperimentHistories_ExperimentPreprocessHistoryId",
                table: "ExperimentHistories");

            migrationBuilder.DropColumn(
                name: "DataSetVersionId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropColumn(
                name: "OutputDataSetId",
                table: "ExperimentPreprocessHistories");

            migrationBuilder.DropColumn(
                name: "ExperimentPreprocessHistoryId",
                table: "ExperimentHistories");

            migrationBuilder.RenameColumn(
                name: "ExperimentPreprocessedHistoryId",
                table: "ExperimentPreprocessHistoryOutput",
                newName: "ExperimentHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_ExperimentPreprocessedHis~",
                table: "ExperimentPreprocessHistoryOutput",
                newName: "IX_ExperimentPreprocessHistoryOutput_ExperimentHistoryId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExperimentPreprocessHistories",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                table: "ExperimentPreprocessHistories",
                column: "DataSetId",
                principalTable: "AquariumDatasetVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
