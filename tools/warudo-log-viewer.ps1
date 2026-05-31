param(
    [int]$Port = 8765,
    [string]$LogPath = "",
    [int]$TailLines = 2000
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($LogPath)) {
    $LogPath = Join-Path $env:USERPROFILE "AppData\LocalLow\HakuyaLabs\Warudo\Player.log"
}

Add-Type -AssemblyName System.Web

$script:IngestedLines = New-Object System.Collections.Generic.List[string]
$script:IngestedLimit = 3000

function Write-TextResponse {
    param(
        [System.Net.HttpListenerContext]$Context,
        [string]$Text,
        [string]$ContentType = "text/plain; charset=utf-8",
        [int]$StatusCode = 200
    )

    $bytes = [System.Text.Encoding]::UTF8.GetBytes($Text)
    $Context.Response.StatusCode = $StatusCode
    $Context.Response.ContentType = $ContentType
    $Context.Response.ContentEncoding = [System.Text.Encoding]::UTF8
    $Context.Response.ContentLength64 = $bytes.Length
    $Context.Response.OutputStream.Write($bytes, 0, $bytes.Length)
    $Context.Response.OutputStream.Close()
}

function Get-LogPayload {
    param(
        [string]$Path,
        [int]$MaxLines
    )

    $exists = Test-Path -LiteralPath $Path -PathType Leaf
    $text = ""
    $length = 0
    $updated = $null
    $errorMessage = ""

    if ($exists) {
        try {
            $item = Get-Item -LiteralPath $Path
            $length = $item.Length
            $updated = $item.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
            $lines = Get-Content -LiteralPath $Path -Tail $MaxLines -Encoding UTF8 -ErrorAction Stop
            $text = ($lines -join "`n")
        } catch {
            $errorMessage = $_.Exception.Message
        }
    }

    $ingestedText = ""
    if ($script:IngestedLines.Count -gt 0) {
        $ingestedText = ($script:IngestedLines -join "`n")
    }
    if (-not [string]::IsNullOrWhiteSpace($ingestedText)) {
        if ([string]::IsNullOrWhiteSpace($text)) {
            $text = $ingestedText
        } else {
            $text = $text + "`n" + $ingestedText
        }
    }

    return [pscustomobject]@{
        path = $Path
        exists = $exists
        text = $text
        bytes = $length
        updated = $updated
        tailLines = $MaxLines
        ingestedLines = $script:IngestedLines.Count
        error = $errorMessage
        servedAt = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
    }
}

function Add-IngestedLogLine {
    param([string]$Line)

    if ([string]::IsNullOrWhiteSpace($Line)) {
        return
    }

    $script:IngestedLines.Add($Line)
    while ($script:IngestedLines.Count -gt $script:IngestedLimit) {
        $script:IngestedLines.RemoveAt(0)
    }
}

function Read-RequestBody {
    param([System.Net.HttpListenerRequest]$Request)

    $encoding = $Request.ContentEncoding
    if ($encoding -eq $null) {
        $encoding = [System.Text.Encoding]::UTF8
    }
    $reader = [System.IO.StreamReader]::new($Request.InputStream, $encoding)
    try {
        return $reader.ReadToEnd()
    } finally {
        $reader.Close()
    }
}

function Format-IngestedBody {
    param([string]$Body)

    $level = "Log"
    $tag = "Node68"
    $message = $Body

    try {
        $data = $Body | ConvertFrom-Json -ErrorAction Stop
        if ($data.level) { $level = [string]$data.level }
        if ($data.tag) { $tag = [string]$data.tag }
        if ($data.message -ne $null) { $message = [string]$data.message }
    } catch {
        # Plain text body is allowed.
    }

    $stamp = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss.fff")
    return "[$stamp] [$level] [$tag] $message"
}

$html = @'
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Warudo 로그 뷰어</title>
  <style>
    @font-face {
      font-family: "D2CodingLocal";
      src:
        local("D2Coding"),
        local("D2Coding ligature"),
        local("D2CodingLigature");
    }
    :root {
      color-scheme: dark;
      --bg: #28211c;
      --panel: #2f2822;
      --panel2: #342c26;
      --panel3: #241e1a;
      --text: #baae9e;
      --muted: #8a8986;
      --line: #43392f;
      --accent: #7fa36a;
      --warn: #d8cb83;
      --error: #c77d64;
      --log: #aaa196;
      --log-dim: #817a71;
      --field: #211c18;
    }
    html[data-theme="cursor-dark"] {
      --bg: #141414;
      --panel: #181818;
      --panel2: #202020;
      --panel3: #101010;
      --text: #d6d6d6;
      --muted: #858585;
      --line: #2d2d2d;
      --accent: #7aa2f7;
      --warn: #d7ba7d;
      --error: #f48771;
      --log: #c8c8c8;
      --log-dim: #7d7d7d;
      --field: #111111;
    }
    html[data-theme="dark-plus"] {
      --bg: #1e1e1e;
      --panel: #252526;
      --panel2: #2d2d30;
      --panel3: #181818;
      --text: #cccccc;
      --muted: #858585;
      --line: #3c3c3c;
      --accent: #4ec9b0;
      --warn: #dcdcaa;
      --error: #f48771;
      --log: #c8c8c8;
      --log-dim: #808080;
      --field: #1b1b1b;
    }
    html[data-theme="tomorrow-blue"] {
      --bg: #002451;
      --panel: #002b60;
      --panel2: #00346f;
      --panel3: #001b3d;
      --text: #dbe8ff;
      --muted: #93a8c8;
      --line: #17446f;
      --accent: #99ffff;
      --warn: #ffeead;
      --error: #ff9da4;
      --log: #c9d9f0;
      --log-dim: #8da0be;
      --field: #001f47;
    }
    html[data-theme="red"] {
      --bg: #211617;
      --panel: #2a1b1d;
      --panel2: #342124;
      --panel3: #1a1112;
      --text: #e0c5c0;
      --muted: #9a817c;
      --line: #4a3032;
      --accent: #ffb86c;
      --warn: #f1c66b;
      --error: #ff6e6e;
      --log: #d2b7b2;
      --log-dim: #8a706b;
      --field: #1c1213;
    }
    html[data-theme="vs-light"] {
      color-scheme: light;
      --bg: #f3f3f3;
      --panel: #ffffff;
      --panel2: #f7f7f7;
      --panel3: #eeeeee;
      --text: #1f1f1f;
      --muted: #696969;
      --line: #d4d4d4;
      --accent: #007acc;
      --warn: #8a6d00;
      --error: #c42b1c;
      --log: #2b2b2b;
      --log-dim: #707070;
      --field: #ffffff;
    }
    * { box-sizing: border-box; }
    html {
      height: 100%;
      overflow: hidden;
    }
    body {
      margin: 0;
      background: var(--bg);
      color: var(--text);
      font-family: "Segoe UI", "Malgun Gothic", Arial, sans-serif;
      height: 100vh;
      height: 100dvh;
      overflow: hidden;
      display: flex;
      flex-direction: column;
    }
    header {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 8px 12px;
      border-bottom: 1px solid var(--line);
      background: var(--panel);
      flex-wrap: wrap;
    }
    h1 {
      font-size: 14px;
      margin: 0 8px 0 0;
      font-weight: 650;
      white-space: nowrap;
    }
    input, select, button {
      background: var(--field);
      color: var(--text);
      border: 1px solid var(--line);
      border-radius: 4px;
      height: 30px;
      padding: 0 9px;
      font: inherit;
    }
    input:focus, select:focus, button:focus-visible {
      outline: 1px solid #6f8f5c;
      outline-offset: 1px;
    }
    input { min-width: 260px; flex: 1; }
    button {
      cursor: pointer;
      color: #c7d3b9;
      border-color: #4c6240;
      background: #303b29;
    }
    button.secondary {
      color: var(--text);
      border-color: var(--line);
      background: var(--field);
    }
    button.danger {
      color: #d7b6aa;
      border-color: #684438;
      background: #3f2b25;
    }
    button.active {
      border-color: #667d56;
      background: #363426;
    }
    label {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      color: var(--muted);
      font-size: 12px;
      white-space: nowrap;
    }
    label input {
      min-width: 0;
      width: 16px;
      height: 16px;
      padding: 0;
    }
    #meta {
      display: flex;
      gap: 16px;
      align-items: center;
      padding: 8px 14px;
      border-bottom: 1px solid var(--line);
      background: var(--panel3);
      color: var(--muted);
      font-size: 12px;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    #meta span:first-child {
      overflow: hidden;
      text-overflow: ellipsis;
    }
    #stats {
      display: flex;
      gap: 8px;
      align-items: center;
      padding: 7px 12px;
      border-bottom: 1px solid var(--line);
      background: #2a231e;
      overflow-x: auto;
    }
    .chip {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      height: 27px;
      padding: 0 9px;
      border: 1px solid var(--line);
      border-radius: 4px;
      background: var(--panel2);
      color: var(--text);
      white-space: nowrap;
      font-size: 12px;
    }
    .chip button {
      height: 22px;
      padding: 0 7px;
      border-radius: 3px;
      background: transparent;
      border-color: transparent;
      color: inherit;
    }
    .chip.active {
      border-color: #667d56;
      background: #363426;
    }
    .chip.warning { color: var(--warn); }
    .chip.error { color: var(--error); }
    main {
      flex: 1 1 0;
      min-height: 0;
      overflow: auto;
      padding: 6px 0 96px;
      scroll-padding-bottom: 96px;
      font-family: "D2CodingLocal", D2Coding, Consolas, "Cascadia Mono", "Malgun Gothic", monospace;
      font-size: 12px;
      line-height: 1.46;
      -webkit-font-smoothing: antialiased;
      text-rendering: optimizeLegibility;
    }
    .entry {
      padding: 4px 12px 5px;
      border-bottom: 1px solid rgba(186,174,158,0.045);
      border-left: 2px solid transparent;
      white-space: pre-wrap;
      word-break: break-word;
      color: var(--log);
    }
    .entry .summary {
      margin-bottom: 4px;
    }
    .entry.warning {
      border-left-color: var(--warn);
      background: rgba(216,203,131,0.035);
    }
    .entry.error {
      border-left-color: var(--error);
      background: rgba(199,125,100,0.045);
    }
    .empty {
      padding: 24px 14px;
      color: var(--muted);
      font-family: "Segoe UI", Arial, sans-serif;
    }
    mark {
      background: rgba(249,238,152,0.18);
      color: #e6dccb;
      padding: 0 2px;
      border-radius: 2px;
    }
    @media (max-width: 800px) {
      header { height: auto; }
      input { min-width: 180px; flex-basis: 100%; }
    }
  </style>
</head>
<body>
  <header>
    <h1>Warudo 로그 뷰어</h1>
    <select id="theme" title="테마">
      <option value="d2-brown">D2 Brown</option>
      <option value="cursor-dark">Cursor Dark</option>
      <option value="dark-plus">Default Dark+</option>
      <option value="tomorrow-blue">Tomorrow Night Blue</option>
      <option value="red">Red</option>
      <option value="vs-light">Visual Studio 2019 Light</option>
    </select>
    <input id="filter" placeholder="검색어 입력 예: Node68/Camera, Error, Exception">
    <select id="level">
      <option value="all">전체</option>
      <option value="warning">경고 이상</option>
      <option value="error">오류/예외</option>
    </select>
    <label><input id="regex" type="checkbox"> 정규식</label>
    <label><input id="invert" type="checkbox"> 제외 검색</label>
    <label><input id="auto" type="checkbox" checked> 자동 새로고침</label>
    <label><input id="scroll" type="checkbox" checked> 아래 따라가기</label>
    <button id="refresh">새로고침</button>
    <button id="copy" class="secondary">보이는 로그 복사</button>
    <button id="clear" class="danger">화면 비우기</button>
  </header>
  <div id="stats">
    <span class="chip active" data-level="all"><button>전체</button><b id="totalCount">0</b></span>
    <span class="chip warning" data-level="warning"><button>경고 이상</button><b id="warningCount">0</b></span>
    <span class="chip error" data-level="error"><button>오류</button><b id="errorCount">0</b></span>
    <span class="chip"><span>표시 중</span><b id="shownCount">0</b></span>
  </div>
  <div id="meta">
    <span id="path">불러오는 중...</span>
    <span id="count"></span>
    <span id="updated"></span>
  </div>
  <main id="log"><div class="empty">Player.log 불러오는 중...</div></main>
  <script>
    const logEl = document.getElementById("log");
    const themeEl = document.getElementById("theme");
    const filterEl = document.getElementById("filter");
    const levelEl = document.getElementById("level");
    const regexEl = document.getElementById("regex");
    const invertEl = document.getElementById("invert");
    const autoEl = document.getElementById("auto");
    const scrollEl = document.getElementById("scroll");
    const pathEl = document.getElementById("path");
    const countEl = document.getElementById("count");
    const updatedEl = document.getElementById("updated");
    const totalCountEl = document.getElementById("totalCount");
    const warningCountEl = document.getElementById("warningCount");
    const errorCountEl = document.getElementById("errorCount");
    const shownCountEl = document.getElementById("shownCount");
    let rawText = "";
    let timer = null;
    let clearedUntil = 0;
    let shownEntries = [];

    function applyTheme(theme) {
      const selected = theme || "d2-brown";
      document.documentElement.dataset.theme = selected;
      themeEl.value = selected;
      localStorage.setItem("warudo-log-theme", selected);
    }

    function classify(entry) {
      if (entry.includes("[Error]") || entry.includes("[Exception]") || entry.includes("[Assert]") || entry.includes("Exception:")) return "error";
      if (entry.includes("[Warning]") || entry.includes("Warning:")) return "warning";
      return "log";
    }

    function splitEntries(text) {
      const lines = text ? text.split(/\n/) : [];
      const entries = [];
      let current = "";
      for (const line of lines) {
        if (/^\[\d{4}-\d{2}-\d{2} /.test(line) && current) {
          entries.push(current);
          current = line;
        } else {
          current = current ? current + "\n" + line : line;
        }
      }
      if (current) entries.push(current);
      return entries;
    }

    function escapeHtml(value) {
      return value.replace(/[&<>"']/g, ch => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;"
      })[ch]);
    }

    function highlight(value, needle) {
      const safe = escapeHtml(value);
      if (!needle) return safe;
      try {
        const pattern = regexEl.checked ? needle : needle.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        return safe.replace(new RegExp(pattern, "gi"), match => "<mark>" + match + "</mark>");
      } catch {
        return safe;
      }
    }

    function matchesNeedle(entry, needle) {
      if (!needle) return true;
      if (!regexEl.checked) return entry.toLowerCase().includes(needle.toLowerCase());
      try {
        return new RegExp(needle, "i").test(entry);
      } catch {
        return false;
      }
    }

    function setLevel(level) {
      levelEl.value = level;
      document.querySelectorAll(".chip[data-level]").forEach(chip => {
        chip.classList.toggle("active", chip.dataset.level === level);
      });
      render();
    }

    function buildEntryHtml(entry, needle) {
      const kind = classify(entry);
      return '<div class="entry ' + kind + '">' + highlight(entry, needle) + '</div>';
    }

    function render() {
      const needle = filterEl.value.trim();
      const level = levelEl.value;
      const allEntries = splitEntries(rawText).slice(clearedUntil);
      const totals = splitEntries(rawText).reduce((acc, entry) => {
        const kind = classify(entry);
        acc.total++;
        if (kind === "warning" || kind === "error") acc.warning++;
        if (kind === "error") acc.error++;
        return acc;
      }, { total: 0, warning: 0, error: 0 });

      totalCountEl.textContent = totals.total;
      warningCountEl.textContent = totals.warning;
      errorCountEl.textContent = totals.error;

      shownEntries = allEntries.filter(entry => {
        const kind = classify(entry);
        if (level === "error" && kind !== "error") return false;
        if (level === "warning" && kind !== "warning" && kind !== "error") return false;
        const matched = matchesNeedle(entry, needle);
        if ((!invertEl.checked && !matched) || (invertEl.checked && matched)) return false;
        return true;
      });

      shownCountEl.textContent = shownEntries.length;
      countEl.textContent = shownEntries.length + "개 표시";
      if (!shownEntries.length) {
        logEl.innerHTML = '<div class="empty">조건에 맞는 로그가 없습니다.</div>';
        return;
      }

      logEl.innerHTML = shownEntries.map(entry => buildEntryHtml(entry, needle)).join("");

      if (scrollEl.checked) {
        logEl.scrollTop = logEl.scrollHeight;
      }
    }

    async function refresh() {
      try {
        const res = await fetch("/api/log", { cache: "no-store" });
        const payload = await res.json();
        pathEl.textContent = payload.exists ? payload.path : "파일 없음: " + payload.path;
        updatedEl.textContent = payload.updated ? "수정됨 " + payload.updated : payload.error || "";
        rawText = payload.text || "";
        render();
      } catch (err) {
        pathEl.textContent = "뷰어 오류";
        updatedEl.textContent = String(err);
      }
    }

    function setTimer() {
      if (timer) clearInterval(timer);
      timer = autoEl.checked ? setInterval(refresh, 1000) : null;
    }

    document.getElementById("refresh").addEventListener("click", refresh);
    themeEl.addEventListener("change", () => applyTheme(themeEl.value));
    document.getElementById("copy").addEventListener("click", async () => {
      await navigator.clipboard.writeText(shownEntries.join("\n\n"));
    });
    document.getElementById("clear").addEventListener("click", () => {
      clearedUntil = splitEntries(rawText).length;
      render();
    });
    document.querySelectorAll(".chip[data-level] button").forEach(button => {
      button.addEventListener("click", () => setLevel(button.parentElement.dataset.level));
    });
    filterEl.addEventListener("input", render);
    levelEl.addEventListener("change", () => setLevel(levelEl.value));
    regexEl.addEventListener("change", render);
    invertEl.addEventListener("change", render);
    autoEl.addEventListener("change", setTimer);
    window.addEventListener("keydown", event => {
      if (event.key === "/" && document.activeElement !== filterEl) {
        event.preventDefault();
        filterEl.focus();
        filterEl.select();
      }
      if (event.key === "Escape" && document.activeElement === filterEl) {
        filterEl.value = "";
        filterEl.blur();
        render();
      }
    });
    setTimer();
    applyTheme(localStorage.getItem("warudo-log-theme") || "d2-brown");
    refresh();
  </script>
</body>
</html>
'@

$listener = [System.Net.HttpListener]::new()
$prefix = "http://127.0.0.1:$Port/"
$listener.Prefixes.Add($prefix)

try {
    $listener.Start()
} catch {
    Write-Host "Failed to start listener on $prefix"
    Write-Host $_.Exception.Message
    exit 1
}

Write-Host "Warudo Log Viewer running at $prefix"
Write-Host "Reading: $LogPath"
Write-Host "Press Ctrl+C to stop."

try {
    while ($listener.IsListening) {
        $context = $listener.GetContext()
        $requestPath = $context.Request.Url.AbsolutePath

        if ($requestPath -eq "/" -or $requestPath -eq "/index.html") {
            Write-TextResponse -Context $context -Text $html -ContentType "text/html; charset=utf-8"
            continue
        }

        if ($requestPath -eq "/api/log") {
            $max = $TailLines
            $queryMax = $context.Request.QueryString["tail"]
            if ([int]::TryParse($queryMax, [ref]$max) -eq $false) {
                $max = $TailLines
            }
            $max = [Math]::Max(100, [Math]::Min($max, 20000))
            $payload = Get-LogPayload -Path $LogPath -MaxLines $max
            $json = $payload | ConvertTo-Json -Depth 4
            Write-TextResponse -Context $context -Text $json -ContentType "application/json; charset=utf-8"
            continue
        }

        if ($requestPath -eq "/api/ingest") {
            if ($context.Request.HttpMethod -ne "POST") {
                Write-TextResponse -Context $context -Text "Method not allowed" -StatusCode 405
                continue
            }

            $body = Read-RequestBody -Request $context.Request
            $line = Format-IngestedBody -Body $body
            Add-IngestedLogLine -Line $line
            Write-TextResponse -Context $context -Text '{"ok":true}' -ContentType "application/json; charset=utf-8"
            continue
        }

        if ($requestPath -eq "/favicon.ico") {
            Write-TextResponse -Context $context -Text "" -StatusCode 204
            continue
        }

        Write-TextResponse -Context $context -Text "Not found" -StatusCode 404
    }
} finally {
    if ($listener.IsListening) {
        $listener.Stop()
    }
    $listener.Close()
}
