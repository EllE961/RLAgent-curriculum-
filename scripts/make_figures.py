"""Draw docs/assets/reward_curve.png from the exported training summaries.

Run from the repo root:  python scripts/make_figures.py

Input: docs/runs/ppo_smoke.csv, the TensorBoard scalars of one mlagents-learn run (one row per summary,
written every 6,000 steps). The dashed reference line is the running-average episode reward stored in
Assets/ML-Agents/Timers/SampleScene_timers.json by the July 2024 editor session.
"""
import csv
import json
import os

import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt
from matplotlib.colors import LinearSegmentedColormap

BG, PANEL = "#0d0c0b", "#131211"
INK, INK2, INK3 = "#ede7de", "#aaa299", "#7a736c"
GRID = "#24221f"
EMBER, TEMPER, PURPLE, GREEN = "#d95926", "#3987e5", "#9085e9", "#199e70"
SERIES = [EMBER, TEMPER, PURPLE, GREEN, "#e8b44c", "#c0509c"]
HEAT = LinearSegmentedColormap.from_list("heat", ["#141110", "#2a1410", "#4b170c", "#7a1f0a", "#ad320c",
                                                  "#d9520f", "#f07a1e", "#f9a63b", "#fccf6b", "#fff0c2"])
plt.rcParams.update({
    "figure.facecolor": BG, "axes.facecolor": PANEL, "savefig.facecolor": BG,
    "axes.edgecolor": GRID, "axes.labelcolor": INK2, "axes.titlecolor": INK, "axes.titleweight": "semibold",
    "axes.titlesize": 12.5, "axes.titlelocation": "left", "axes.titlepad": 12, "axes.labelsize": 10,
    "axes.spines.top": False, "axes.spines.right": False, "axes.spines.left": False,
    "axes.grid": True, "axes.grid.axis": "y", "grid.color": GRID, "grid.linewidth": 0.8, "grid.linestyle": "-",
    "xtick.color": INK3, "ytick.color": INK3, "xtick.labelsize": 9, "ytick.labelsize": 9,
    "xtick.major.size": 0, "ytick.major.size": 0, "xtick.major.pad": 6, "ytick.major.pad": 6,
    "axes.axisbelow": True, "font.family": ["Segoe UI", "DejaVu Sans"], "lines.linewidth": 2,
    "lines.solid_capstyle": "round", "legend.frameon": False, "legend.fontsize": 9.5, "legend.labelcolor": INK2,
    "figure.dpi": 110, "savefig.dpi": 200,
})

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
RUN = os.path.join(ROOT, "docs", "runs", "ppo_smoke.csv")
TIMERS = os.path.join(ROOT, "Assets", "ML-Agents", "Timers", "SampleScene_timers.json")
OUT = os.path.join(ROOT, "docs", "assets")


def load_run():
    with open(RUN, newline="") as f:
        rows = [r for r in csv.DictReader(f) if r.get("Environment/Cumulative Reward")]
    return rows


def main():
    rows = load_run()
    steps = [int(r["step"]) / 1000 for r in rows]
    mean = [float(r["Environment/Cumulative Reward"]) for r in rows]
    gauge = json.load(open(TIMERS))["gauges"]["Collecter.CumulativeReward"]
    ref = gauge["runningAverage"]
    print(f"{len(rows)} summaries, steps {steps[0]:.0f}K..{steps[-1]:.0f}K, reward {mean[0]:.2f} -> {mean[-1]:.2f}, "
          f"best {max(mean):.2f}; 2024 running average {ref:.2f} over {gauge['count']} episodes")

    fig, ax = plt.subplots(figsize=(9.6, 4.4))
    ax.plot(steps, mean, color=EMBER, marker="o", ms=3.5, label="this run, mean reward per 6K-step summary")
    ax.axhline(ref, color=TEMPER, lw=1.4, ls=(0, (5, 4)),
               label=f"July 2024 editor session, running average over {gauge['count']:,} episodes")
    ax.axhline(0, color=INK3, lw=0.8)
    ax.annotate(f"{mean[0]:.1f}", (steps[0], mean[0]), xytext=(8, -2), textcoords="offset points",
                color=INK2, fontsize=9, va="center")
    ax.annotate(f"{mean[-1]:.2f}", (steps[-1], mean[-1]), xytext=(8, 0), textcoords="offset points",
                color=INK2, fontsize=9, va="center")
    ax.set_xlabel("trainer steps, thousands (agent decisions summed over 12 agents)")
    ax.set_ylabel("episode reward")
    ax.set_xlim(0, steps[-1] * 1.07)
    ax.set_title("Mean episode reward during a 13-minute PPO run", pad=28)
    ax.legend(loc="lower left", bbox_to_anchor=(-0.01, 0.995), ncol=2, fontsize=8.8)
    fig.text(0.012, -0.03, "Each point averages the episodes that finished since the previous summary. Episodes "
             "last 3,000 physics steps (500 decisions); +1 per coin (6 per arena), -1 per obstacle or wall hit, -1 spread over the episode.",
             color=INK3, fontsize=8.5, ha="left", va="top")
    os.makedirs(OUT, exist_ok=True)
    path = os.path.join(OUT, "reward_curve.png")
    fig.savefig(path, bbox_inches="tight", pad_inches=0.25)
    print(f"wrote {path} ({os.path.getsize(path) / 1024:.0f} KB)")


if __name__ == "__main__":
    main()
