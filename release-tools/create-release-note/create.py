#!/usr/bin/env python
import datetime
from github import Github

def getIssues(label):
    return repo.get_issues(state='closed',labels=[label],sort='created',direction='asc',milestone=milestone)

def printIssues(title, label):
    issues = getIssues(label)
    if issues.totalCount > 0:
        print(title)
        for _issue in issues:
            print(' - #{:<4} {}'.format(_issue.number, _issue.title))

if __name__ == '__main__':
    #　XXXにはGitHubのトークンを入れる
    token = 'XXX'
    
    g = Github(token)

    # kamonohashiリポジトリの取得 
    for _repo in g.get_organization('kamonohashi').get_repos(type='public'):
        if _repo.name == 'kamonohashi':
            repo = _repo
            break

    # milestone一覧の取得と選択
    open_milestones = repo.get_milestones(state='open')
    for milestone in open_milestones:
        print(milestone)

    milestone_number = int(input("取得したいmilestoneのnumber: "))
    milestone = repo.get_milestone(number=milestone_number)

    print(repo.get_issues)
    # コピー用文章を出力
    print('## ' + milestone.title + ' / ' + datetime.date.today().strftime('%Y-%m-%d'))
    print('<!-- かもっちイラストを挿入 -->')

    #　Issueラベル(kind/***)ごとに出力
    label = repo.get_label('kind/feature')
    printIssues('## New Feature ', label)

    label = repo.get_label('kind/enhancement')
    printIssues('## Enhancement ', label)

    label = repo.get_label('kind/bug')
    printIssues('## Bug Fixes ', label)

    label = repo.get_label('kind/design')
    printIssues('## Design', label)
    
    label = repo.get_label('kind/refactoring')
    printIssues('## Refactoring', label)
