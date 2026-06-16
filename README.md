# Projeto | Body Calculator (Calculadora Nutricional e Metabólica)

Projeto MVP acadêmico desenvolvido para as disciplinas de Sistemas Distribuídos, Frontend e Backend.

O sistema consiste em uma calculadora de saúde avançada com rede social integrada, que realiza:
- **Cálculo do Metabolismo Basal e Gasto Energético Total (GET)** utilizando fórmulas científicas (Mifflin-St Jeor, Harris-Benedict, etc.).
- **Planejamento de Macronutrientes** (Proteínas, Carboidratos e Gorduras) adaptado ao objetivo físico do usuário (Emagrecimento, Manutenção ou Ganho de Massa).
- **Metas de Hidratação** baseadas na faixa etária e peso corporal.
- **Feed Social da Comunidade:** Um espaço interativo em tempo real para os usuários compartilharem resultados, dúvidas e acompanharem o progresso uns dos outros.

Para a disciplina de <u>Frontend e Backend</u>, os integrantes são: Daniel Costa | Stanley Dias | Pedro Henrique | Hiago Paes<br>
Para a disciplina de <u>Sistemas Distribuídos</u>, os integrantes são: Daniel Costa | Stanley Dias | Pedro Henrique | Hiago Paes | Eduardo Mendonça

Sobre os commits serem somente de uma pessoa, isso se dá pelo modo de desenvolvimento que escolhemos: programamos juntos em tempo real utilizando da extensão Live Share no VS Code, que permite a conexão e programação em tempo real no projeto de uma pessoa. Dessa forma, ao final de cada progressão no código, realizamos o commit para o Github para salvarmos tudo no repositório

## 🏗️ Arquitetura do Projeto

O projeto está estruturado no padrão *Monorepo*, operando como uma aplicação Web distribuída completa (Full-Stack). A arquitetura é dividida em:
- **Backend:** API RESTful robusta construída em **C# (.NET)**, responsável pelo motor matemático de cálculos e regras de negócio estritas.
- **Frontend:** Single Page Application (SPA) reativa e dinâmica construída com **React (Vite)**.
- **Banco de Dados & Autenticação:** Backend-as-a-Service (BaaS) utilizando **Supabase** (PostgreSQL) com Segurança a Nível de Linha (RLS) e login social unificado via Google.


## 🚀 Como Executar o Projeto Localmente

Se você deseja clonar e rodar este projeto em sua máquina, siga os passos abaixo:

### Pré-requisitos
Certifique-se de ter as seguintes ferramentas instaladas:
- [Node.js](https://nodejs.org/) (Necessário para rodar o React/Vite no frontend)
- [.NET SDK](https://dotnet.microsoft.com/download) (Necessário para compilar e rodar a API em C#)
- Conta ativa no [Supabase](https://supabase.com/) para hospedar o banco de dados.

### 1. Configurando o Banco de Dados (Supabase)
Crie um novo projeto no Supabase, acesse a aba **SQL Editor** e execute os comandos de criação das tabelas e políticas de segurança:

```sql
-- Insira aqui os scripts SQL da tabela "perfis"
create table public.perfis (
  id uuid not null,
  nome text not null,
  idade integer not null,
  sexo text not null,
  peso double precision not null,
  altura double precision not null,
  percentual_gordura double precision null,
  fator_atividade double precision not null,
  objetivo_fisico text not null,
  atualizado_em timestamp with time zone null default now(),
  username text not null,
  constraint perfis_pkey primary key (id),
  constraint perfis_username_key unique (username),
  constraint perfis_id_fkey foreign KEY (id) references auth.users (id),
  constraint perfis_objetivo_fisico_check check (
    (
      objetivo_fisico = any (
        array[
          'Manutencao'::text,
          'Emagrecimento'::text,
          'GanhoDeMassa'::text
        ]
      )
    )
  ),
  constraint perfis_altura_check check ((altura > (0)::double precision)),
  constraint perfis_peso_check check ((peso > (0)::double precision)),
  constraint perfis_sexo_check check (
    (
      sexo = any (array['Masculino'::text, 'Feminino'::text])
    )
  ),
  constraint perfis_percentual_gordura_check check (
    (
      (percentual_gordura >= (0)::double precision)
      and (percentual_gordura <= (100)::double precision)
    )
  ),
  constraint perfis_fator_atividade_check check ((fator_atividade >= (1.2)::double precision)),
  constraint perfis_idade_check check ((idade >= 0))
) TABLESPACE pg_default;

-- Insira aqui os scripts SQL da tabela "postagens" e as regras de RLS
create table public.postagens (
  id uuid not null default gen_random_uuid (),
  perfil_id uuid not null,
  conteudo text not null,
  criado_em timestamp with time zone null default now(),
  constraint postagens_pkey primary key (id),
  constraint postagens_perfil_id_fkey foreign KEY (perfil_id) references perfis (id) on delete CASCADE,
  constraint postagens_conteudo_check check (
    (
      (char_length(conteudo) > 0)
      and (char_length(conteudo) <= 500)
    )
  )
) TABLESPACE pg_default;

-- Segurança RLS (Row Level Security) para Perfis
alter table public.perfis enable row level security;

create policy "Qualquer utilizador logado pode ver perfis"
on public.perfis for select
using ( auth.uid() is not null );

-- Segurança RLS (Row Level Security) para Postagens
alter table public.postagens enable row level security;

create policy "Todos podem ver as postagens"
on public.postagens for select
using (true);

create policy "Utilizadores podem inserir as suas próprias postagens"
on public.postagens for insert
with check ( auth.uid() = perfil_id );
```

### 2. Configurando as Variáveis de Ambiente (Frontend)
Na pasta /frontend, crie um arquivo chamado .env na raiz da pasta e adicione as chaves de acesso fornecidas pelo seu projeto no Supabase:

(Para o professor, será enviado o arquivo .env com as chaves do nosso Supabase para utilização)

```
VITE_SUPABASE_URL=sua_url_do_projeto_aqui
VITE_SUPABASE_ANON_KEY=sua_chave_anonima_aqui
```

### 3. Iniciando o Backend (C# .NET)
Abra um terminal, navegue até a pasta do backend e inicie a API:

```
cd backend
dotnet run
```

A API do C# estará escutando as requisições na porta http://localhost:5000 (Verifique as configurações de CORS se houver alteração de porta).


### 4. Iniciando o Frontend (React / Vite)
Abra um novo terminal (mantenha o backend rodando), navegue até a pasta do frontend, instale as dependências e inicie o servidor visual:

```
cd frontend
npm install
npm run dev
```

Acesse http://localhost:5173 no seu navegador para utilizar o sistema.

---

*Feito com carinho.*